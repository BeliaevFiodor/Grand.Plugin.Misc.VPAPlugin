using Grand.Core;
using Grand.Core.Data;
using Grand.Core.Domain.Forums;
using Grand.Core.Domain.Vendors;
using Grand.Plugin.Misc.VPAPlugin.Domains;
using Grand.Plugin.Misc.VPAPlugin.Models;
using Grand.Web.Commands.Models.Vendors;
using Grand.Web.Models.Common;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendor = Grand.Plugin.Misc.VPAPlugin.Domains.Vendor;

namespace Grand.Plugin.Misc.VPAPlugin.Services
{
    public partial class VendorService : Grand.Services.Vendors.VendorService, IVendorService
    {
        private readonly IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Product> _productRepository;
        private readonly IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Vendor> _vendorRepository;
        private readonly IMediator _mediator;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IRepository<Grand.Core.Domain.Customers.Customer> _customerRepository;
        private readonly Grand.Services.Forums.IForumService _forumService;

        public VendorService(IRepository<Grand.Core.Domain.Vendors.Vendor> vendorRepository,
            IRepository<VendorReview> vendorReviewRepository,
            IMediator mediator,
            IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Product> productRepo,
            IRepository<Grand.Plugin.Misc.VPAPlugin.Domains.Vendor> vendorRepo,
            IStoreContext storeContext,
            IWorkContext workContext,
            IRepository<Grand.Core.Domain.Customers.Customer> customerRepository,
            Grand.Services.Forums.IForumService forumService)
            : base(vendorRepository, vendorReviewRepository, mediator)
        {
            _productRepository = productRepo;
            _vendorRepository = vendorRepo;
            _mediator = mediator;
            _storeContext = storeContext;
            _workContext = workContext;
            _customerRepository = customerRepository;
            _forumService = forumService;
        }

        public async Task<Vendor> SetOnFalse(Vendor vendor)
        {
            if (vendor == null)
                throw new NullReferenceException();
            vendor.IsAdminApproveNeeded = true;
            return await _vendorRepository.UpdateAsync(vendor);
        }

        public async Task<Vendor> SetOnTrue(Vendor vendor)
        {
            if (vendor == null)
                throw new NullReferenceException();
            vendor.IsAdminApproveNeeded = true;
            return await _vendorRepository.UpdateAsync(vendor);
        }

        public async Task<List<VendorModel>> GetVendors()
        {
            var result = _vendorRepository.Collection.AsQueryable().ToList();
            return result.Select(x => new VendorModel {
                IsAdminApproveNeeded = x.IsAdminApproveNeeded,
                Name = x.Name,
                _id = x.Id,
                Products = _productRepository.Collection.AsQueryable().Where(y => y.VendorId == x.Id).Select(y => new ProductModel {
                    Id = y.Id,
                    Name = y.Name,
                    Description = y.FullDescription,
                    IsAdminApproved = y.IsAdminApproved
                }).ToList()
            }).ToList();
        }

        public async Task CheckChange(string id)
        {
            var vendor = _vendorRepository.GetById(id);
            vendor.IsAdminApproveNeeded = !vendor.IsAdminApproveNeeded;
            await _vendorRepository.UpdateAsync(vendor);

        }
        public async Task CheckProductChange(string id)
        {
            var product = _productRepository.GetById(id);
            product.IsAdminApproved = !product.IsAdminApproved;
            await _productRepository.UpdateAsync(product);
            
            if (product.IsAdminApproved && !String.IsNullOrEmpty(product.VendorId))
            {
                var vendor = _vendorRepository.GetById(product.VendorId);
                if (vendor.IsAdminApproveNeeded)
                {
                    var customer = _customerRepository.Collection.AsQueryable().Where(x => x.VendorId == product.VendorId).FirstOrDefault();

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
            if (!product.IsAdminApproved && !String.IsNullOrEmpty(product.VendorId))
            {
                var vendor = _vendorRepository.GetById(product.VendorId);
                if (vendor.IsAdminApproveNeeded)
                {
                    var customer = _customerRepository.Collection.AsQueryable().Where(x => x.VendorId == product.VendorId).FirstOrDefault();

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
    }
}
        

        
