using Grand.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.VPAPlugin.Services
{
    public interface IProductService:Grand.Services.Catalog.IProductService
    {
        Task<Grand.Plugin.Misc.VPAPlugin.Domains.Product> Approve(Grand.Plugin.Misc.VPAPlugin.Domains.Product product);
        Task<Grand.Plugin.Misc.VPAPlugin.Domains.Product> DisApprove(Grand.Plugin.Misc.VPAPlugin.Domains.Product product);
        //new Task<IList<Product>> GetAllProductsDisplayedOnHomePage();
    }
}
