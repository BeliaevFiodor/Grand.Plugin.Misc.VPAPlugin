using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Plugin.Misc.VPAPlugin.Domains
{
    public partial class Vendor : Grand.Core.Domain.Vendors.Vendor
    {
        public bool IsAdminApproveNeeded {get;set;}
    }
}
