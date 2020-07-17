using Grand.Core.Plugins;
using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Grand.Plugin.Misc.VPAPlugin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.VPAPlugin.Controllers
{
    [Area("Admin")]
    [AuthorizeAdmin]
    public class VPAPluginController: BasePluginController
    {
        private readonly IVendorService _vendorService;
        public VPAPluginController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }
        public async Task<IActionResult> Admin()
        {
            var a = await _vendorService.GetVendors();
            return View("~/Plugins/Misc.VPAPlugin/Views/Admin.cshtml", a);
        }
        public async Task<IActionResult> CheckChange(string id)
        {
            await _vendorService.CheckChange(id);
            return Ok();
        }

        public async Task<IActionResult> CheckProductChange(string id)
        {
            await _vendorService.CheckProductChange(id);
            return Ok();
        }
    }
}
