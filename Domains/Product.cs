using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Plugin.Misc.VPAPlugin.Domains
{
    public partial class Product:Grand.Core.Domain.Catalog.Product
    {
        public bool IsAdminApproved { get; set; }
    }
}
