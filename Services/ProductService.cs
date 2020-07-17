using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Data;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.Seo;
using Grand.Core.Domain.Vendors;
using Grand.Plugin.Misc.VPAPlugin.Extension;
using Grand.Services.Catalog;
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
    public class ProductService: Grand.Services.Catalog.ProductService,IProductService
    {
        private readonly IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Product> _repo;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Vendor> _vendorService;
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
            IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Vendor> vendorService) : base(cacheManager,productRepository,productReviewRepository,urlRecordRepository,customerRepository,
                                                customerRoleProductRepository,customerTagProductRepository,productDeletedRepository,customerProductRepository,productTagRepository,
                                                productAttributeService,productAttributeParser,workContext,mediator,aclService,storeMappingService,catalogSettings)
        {
            _repo = repo;
            _aclService = aclService;
            _storeMappingService = storeMappingService;
            _vendorService = vendorService;
        }

        public async Task<Grand.Plugin.Misc.VPAPlugin.Domains.Product> Approve(Grand.Plugin.Misc.VPAPlugin.Domains.Product product)
        {
            if (product == null)
                throw new NullReferenceException();
            product.IsAdminApproved = true;
            return await _repo.UpdateAsync(product);
        }

        public async Task<Grand.Plugin.Misc.VPAPlugin.Domains.Product> DisApprove(Grand.Plugin.Misc.VPAPlugin.Domains.Product product)
        {
            if (product == null)
                throw new NullReferenceException();
            product.IsAdminApproved = false;
            return await _repo.UpdateAsync(product);
        }

       
    }
}
