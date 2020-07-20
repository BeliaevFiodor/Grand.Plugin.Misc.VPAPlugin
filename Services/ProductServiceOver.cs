using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Data;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.Forums;
using Grand.Core.Domain.Seo;
using Grand.Core.Domain.Vendors;
using Grand.Services.Catalog;
using Grand.Services.Forums;
using Grand.Services.Security;
using Grand.Services.Stores;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.VPAPlugin.Services
{
    public partial class ProductServiceOver : Grand.Services.Catalog.ProductService
    {
        private readonly IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Product> _repo;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IRepository<Vendor> _vendorService;
        private readonly IRepository<Customer> _customerService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IForumService _forumService;
        private readonly VPAPluginSettings _settings;
        public ProductServiceOver(IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Product> repo,
            ICacheManager cacheManager,
            IRepository<Product> productRepository,
            IRepository<ProductReview> productReviewRepository,
            IRepository<UrlRecord> urlRecordRepository,
            IRepository<Customer> customerRepository,
            IRepository<CustomerRoleProduct> customerRoleProductRepository,
            IRepository<CustomerTagProduct> customerTagProductRepository,
            IRepository<ProductDeleted> productDeletedRepository,
            IRepository<CustomerProduct> customerProductRepository,
            IRepository<ProductTag> productTagRepository,
            IProductAttributeService productAttributeService,
            IProductAttributeParser productAttributeParser,
            IWorkContext workContext,
            IMediator mediator,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            CatalogSettings catalogSettings,
            IRepository<Vendor> vendorService,
            IStoreContext storeContext,
            IForumService forumService,
            VPAPluginSettings settings) : base(cacheManager, productRepository, productReviewRepository, urlRecordRepository, customerRepository,
                                                customerRoleProductRepository, customerTagProductRepository, productDeletedRepository, customerProductRepository, productTagRepository,
                                                productAttributeService, productAttributeParser, workContext, mediator, aclService, storeMappingService, catalogSettings)
        {
            _repo = repo;
            _aclService = aclService;
            _storeMappingService = storeMappingService;
            _vendorService = vendorService;
            _customerService = customerRepository;
            _storeContext = storeContext;
            _workContext = workContext;
            _forumService = forumService;
            _settings = settings;
        }

        public override async Task<IList<Product>> GetAllProductsDisplayedOnHomePage()
        {
            var builder = Builders<Grand.Plugin.Misc.VPAPlugin.Domains.Product>.Filter;
            var filter = builder.Eq(x => x.Published, true);
            filter &= builder.Eq(x => x.ShowOnHomePage, true);
            filter &= builder.Eq(x => x.VisibleIndividually, true);

            var query = _repo.Collection.Find(filter).SortBy(x => x.DisplayOrder).ThenBy(x => x.Name);

            var products = await query.ToListAsync();

            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();

            //availability dates
            products = products.Where(p => p.IsAvailable()).ToList();

            products = products.Where(x => !(x.VendorId != null && x.IsAdminApproveNeeded && !x.IsAdminApproved))
                        .ToList();
            List<Product> result = new List<Product>();
            foreach (var p in products)
                result.Add(p);
            return result;
        }

        public override async Task InsertProduct(Product product)
        {
            await base.InsertProduct(product);
            var prod = await _repo.GetByIdAsync(product.Id);
            if (!prod.IsAdminApproved && !String.IsNullOrEmpty(prod.VendorId))
            {
                    bool isNew = true;
                    var vendorProducts = _repo.Collection.AsQueryable().FirstOrDefault(x => x.VendorId == prod.VendorId);
                    if (vendorProducts == null)
                        isNew = _settings.IsNewVendorsAdminApproveNeeded;
                    else isNew = vendorProducts.IsAdminApproveNeeded;
               if (isNew)
                {
                    var vendor = _vendorService.GetById(prod.VendorId);
                    var admins = _customerService.Collection.AsQueryable();
                    var admins2 = admins.Where(x => x.CustomerRoles.Any(y => y.SystemName == "Administrators")).ToList();
                    if(admins2!=null && admins2.Count>0)
                    foreach (var admin in admins2)
                    {
                        var privateMessage = new PrivateMessage {
                            StoreId = _storeContext.CurrentStore.Id,
                            ToCustomerId = admin.Id,
                            FromCustomerId = _workContext.CurrentCustomer.Id,
                            Subject = "You need to approve product",
                            Text = $"Vendor {vendor.Name} has added new product {product.Name}. You have to approve it.",
                            IsDeletedByAuthor = false,
                            IsDeletedByRecipient = false,
                            IsRead = false,
                            CreatedOnUtc = DateTime.UtcNow,
                        };

                        await _forumService.InsertPrivateMessage(privateMessage);
                    }
                }
            }
            // return product;
        }
    }
}
