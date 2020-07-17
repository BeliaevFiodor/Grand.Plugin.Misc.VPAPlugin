using Grand.Plugin.Misc.VPAPlugin.Domains;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Plugin.Misc.VPAPlugin.Models
{
    public class VendorModel
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public bool IsAdminApproveNeeded { get; set; }
        public List<ProductModel> Products { get; set; }
        
    }
}
