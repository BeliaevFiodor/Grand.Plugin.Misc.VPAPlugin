using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Data;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.Directory;
using Grand.Core.Domain.Discounts;
using Grand.Core.Domain.Forums;
using Grand.Core.Domain.Localization;
using Grand.Core.Domain.Orders;
using Grand.Core.Domain.Seo;
using Grand.Core.Domain.Tax;
using Grand.Core.Domain.Vendors;
using Grand.Framework.Extensions;
using Grand.Services.Catalog;
using Grand.Services.Customers;
using Grand.Services.Directory;
using Grand.Services.Discounts;
using Grand.Services.Forums;
using Grand.Services.Helpers;
using Grand.Services.Localization;
using Grand.Services.Logging;
using Grand.Services.Media;
using Grand.Services.Orders;
using Grand.Services.Seo;
using Grand.Services.Shipping;
using Grand.Services.Stores;
using Grand.Services.Tax;
using Grand.Services.Vendors;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Interfaces;
using Grand.Web.Areas.Admin.Models.Catalog;
using Grand.Web.Areas.Admin.Models.Orders;
using Grand.Web.Areas.Admin.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Grand.Plugin.Misc.VPAPlugin.Services
{
    public partial class CreateProductModelServiceOver : ProductViewModelService
    {
        private readonly IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Product> _productRepo;
        private readonly IRepository<Domains.Vendor> _vendorRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IForumService _forumService;

        public CreateProductModelServiceOver(
            IRepository<Domains.Product> productRepo,
            IRepository<Domains.Vendor> vendorRepo,
            IMediator mediator,
            IStoreContext storeContext,
            IRepository<Customer> customerRepo,
            IForumService forumService,
       IProductService productService,
       IPictureService pictureService,
       IProductAttributeService productAttributeService,
       IProductTagService productTagService,
       ICurrencyService currencyService,
       IMeasureService measureService,
       IDateTimeHelper dateTimeHelper,
       IManufacturerService manufacturerService,
       ICategoryService categoryService,
       IVendorService vendorService,
       ILocalizationService localizationService,
       IProductTemplateService productTemplateService,
       ISpecificationAttributeService specificationAttributeService,
       IWorkContext workContext,
       IShippingService shippingService,
       IShipmentService shipmentService,
       ITaxCategoryService taxCategoryService,
       IDiscountService discountService,
       ICustomerService customerService,
       IStoreService storeService,
       IUrlRecordService urlRecordService,
       ICustomerActivityService customerActivityService,
       IBackInStockSubscriptionService backInStockSubscriptionService,
       IDownloadService downloadService,
       IProductAttributeParser productAttributeParser,
       ILanguageService languageService,
       IProductAttributeFormatter productAttributeFormatter,
       IServiceProvider serviceProvider,
       CurrencySettings currencySettings,
       MeasureSettings measureSettings,
       TaxSettings taxSettings) : base(productService, pictureService, productAttributeService, productTagService, currencyService, measureService, dateTimeHelper, manufacturerService,
                                categoryService, vendorService, localizationService, productTemplateService, specificationAttributeService, workContext,
                                shippingService, shipmentService, taxCategoryService, discountService, customerService, storeService, urlRecordService, customerActivityService, backInStockSubscriptionService,
                                downloadService, productAttributeParser, languageService, productAttributeFormatter, serviceProvider, currencySettings, measureSettings, taxSettings)
        {
            _productRepo = productRepo;
            _vendorRepo = vendorRepo;
            _mediator = mediator;
            _workContext = workContext;
            _storeContext = _storeContext;
            _customerRepo = customerRepo;
            _forumService = forumService;
        }
        
        }

    }

