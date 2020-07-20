using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Data;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.Forums;
using Grand.Core.Domain.Seo;
using Grand.Core.Domain.Vendors;
using Grand.Plugin.Misc.VPAPlugin.Extension;
using Grand.Plugin.Misc.VPAPlugin.Models;
using Grand.Services.Catalog;
using Grand.Services.Configuration;
using Grand.Services.Forums;
using Grand.Services.Security;
using Grand.Services.Stores;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.VPAPlugin.Services
{
    public class ProductService : Grand.Services.Catalog.ProductService, IProductService
    {
        private readonly IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Product> _repo;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IRepository<Vendor> _vendorService;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IForumService _forumService;
        private IRepository<Vendor> _vendorRepository;
        private readonly VPAPluginSettings _settings;
        

        public ProductService(IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Product> repo,
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
            IRepository<Vendor> vendorRepository,
            VPAPluginSettings settings) : base(cacheManager, productRepository, productReviewRepository, urlRecordRepository, customerRepository,
                                                customerRoleProductRepository, customerTagProductRepository, productDeletedRepository, customerProductRepository, productTagRepository,
                                                productAttributeService, productAttributeParser, workContext, mediator, aclService, storeMappingService, catalogSettings)
        {
            _repo = repo;
            _aclService = aclService;
            _storeMappingService = storeMappingService;
            _vendorService = vendorService;
            _customerRepository = customerRepository;
            _workContext = workContext;
            _storeContext = storeContext;
            _forumService = forumService;
            _vendorRepository = vendorRepository;
            _settings = settings;
            
        }

        //public async Task<Grand.Plugin.Misc.VPAPlugin.Domains.Product> Approve(Grand.Plugin.Misc.VPAPlugin.Domains.Product product)
        //{
        //    if (product == null)
        //        throw new NullReferenceException();
        //    product.IsAdminApproved = true;
        //    return await _repo.UpdateAsync(product);
        //}

        //public async Task<Grand.Plugin.Misc.VPAPlugin.Domains.Product> DisApprove(Grand.Plugin.Misc.VPAPlugin.Domains.Product product)
        //{
        //    if (product == null)
        //        throw new NullReferenceException();
        //    product.IsAdminApproved = false;
        //    return await _repo.UpdateAsync(product);
        //}

        public async Task CheckProductChange(string id)
        {
            var product = _repo.GetById(id);
            product.IsAdminApproved = !product.IsAdminApproved;
            await _repo.UpdateAsync(product);

            if (product.IsAdminApproved && !String.IsNullOrEmpty(product.VendorId) && product.IsAdminApproveNeeded)
            {
                var customer = _customerRepository.Collection.AsQueryable().Where(x => x.VendorId == product.VendorId).FirstOrDefault();
                if (customer != null)
                {
                    var privateMessage = new PrivateMessage {
                        StoreId = _storeContext.CurrentStore.Id,
                        ToCustomerId = customer.Id,
                        FromCustomerId = _workContext.CurrentCustomer.Id,
                        Subject = "Product was approved",
                        Text = $"Administrator {_workContext.CurrentCustomer.SystemName} approve your product {product.Name}.",
                        IsDeletedByAuthor = false,
                        IsDeletedByRecipient = false,
                        IsRead = false,
                        CreatedOnUtc = DateTime.UtcNow,
                    };

                    await _forumService.InsertPrivateMessage(privateMessage);
                }
            }

            if (!product.IsAdminApproved && !String.IsNullOrEmpty(product.VendorId) && product.IsAdminApproveNeeded)
            {
                var vendor = _vendorRepository.GetById(product.VendorId);

                var customer = _customerRepository.Collection.AsQueryable().Where(x => x.VendorId == product.VendorId).FirstOrDefault();
                if (customer != null)
                {
                    var privateMessage = new PrivateMessage {
                        StoreId = _storeContext.CurrentStore.Id,
                        ToCustomerId = customer.Id,
                        FromCustomerId = _workContext.CurrentCustomer.Id,
                        Subject = "Product was disapproved",
                        Text = $"Administrator {_workContext.CurrentCustomer.SystemName} disapprove your product {product.Name}.",
                        IsDeletedByAuthor = false,
                        IsDeletedByRecipient = false,
                        IsRead = false,
                        CreatedOnUtc = DateTime.UtcNow,
                    };

                    await _forumService.InsertPrivateMessage(privateMessage);
                }
            }
        }
        public async Task CheckChange(string id)
        {
            var vendorProducts = _repo.Collection.AsQueryable().Where(x => x.VendorId == id).ToList();
            foreach (var product in vendorProducts)
            {
                product.IsAdminApproveNeeded = !product.IsAdminApproveNeeded;
                _repo.Update(product);
            }
        }
        public async Task<VendorList> GetVendors()
        {
            var result = new VendorList();
            if (_workContext.CurrentCustomer.CustomerRoles.Any(x => x.Name == "Vendors"))
                result.IsVendor = true;
            else result.IsVendor = false;
            var products = _repo.Collection.AsQueryable().Where(x => x.VendorId != null).AsEnumerable().GroupBy(x=>x.VendorId,x=>x).ToDictionary(x=>x.Key)
                            .Select(z=>new VendorModel 
                            {
                                IsAdminApproveNeeded = z.Value.FirstOrDefault().IsAdminApproveNeeded,
                                Name = _vendorRepository.GetById(z.Key).Name,
                                Products = z.Value.Select(y => new ProductModel {
                                    IsAdminApproved = y.IsAdminApproved,
                                    IsAdminApproveNeeded = y.IsAdminApproveNeeded,
                                    Description = y.FullDescription,
                                    Name = y.Name,
                                    Id = y.Id
                                }).ToList(),
                                _id = z.Key
                            }).ToList();
            result.List = products;
            result.IsNewVendorsAdminApproveNeeded = _settings.IsNewVendorsAdminApproveNeeded;
            if (result.IsVendor)
                result.List.RemoveAll(x => x._id != _workContext.CurrentVendor.Id);
            return result;
        }

     
    }

}

