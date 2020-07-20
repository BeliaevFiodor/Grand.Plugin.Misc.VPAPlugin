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
        Task CheckProductChange(string id);
        Task CheckChange(string id);
        Task<VendorList> GetVendors();
    }
}
