using Grand.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Plugin.Misc.VPAPlugin
{
    public class VPAPluginSettings:ISettings
    {
        public bool IsNewVendorsAdminApproveNeeded { get; set; }
    }
}
