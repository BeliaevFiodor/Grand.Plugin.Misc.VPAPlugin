using Grand.Plugin.Misc.VPAPlugin.Domains;
using Grand.Plugin.Misc.VPAPlugin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.VPAPlugin.Services
{
    public interface IVendorService : Grand.Services.Vendors.IVendorService
    {
        //Task<Vendor> SetOnTrue(Vendor vendor);
        //Task<Vendor> SetOnFalse(Vendor vendor);
        Task<List<VendorModel>> GetVendors();
        Task CheckChange(string id);
        Task CheckProductChange(string id);
    }
}
