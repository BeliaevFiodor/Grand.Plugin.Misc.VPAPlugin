using Grand.Core.Domain.Catalog;
using Grand.Plugin.Misc.VPAPlugin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.VPAPlugin.Services
{
    public interface IProductService:Grand.Services.Catalog.IProductService
    {
        //Task<Grand.Plugin.Misc.VPAPlugin.Domains.Product> Approve(Grand.Plugin.Misc.VPAPlugin.Domains.Product product);
        //Task<Grand.Plugin.Misc.VPAPlugin.Domains.Product> DisApprove(Grand.Plugin.Misc.VPAPlugin.Domains.Product product);
        Task CheckProductChange(string id);
        Task CheckChange(string id);
        Task<VendorList> GetVendors();
        //new Task<IList<Product>> GetAllProductsDisplayedOnHomePage();
    }
}
