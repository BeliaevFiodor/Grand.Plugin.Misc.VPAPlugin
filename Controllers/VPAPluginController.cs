using Grand.Core.Plugins;
using Grand.Framework.Controllers;
using Grand.Framework.Mvc.Filters;
using Grand.Plugin.Misc.VPAPlugin.Services;
using Grand.Services.Configuration;
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
        
        private readonly IProductService _productService;
        private readonly VPAPluginSettings _settings;
        private readonly ISettingService _settingService;
        public VPAPluginController(IProductService productService, VPAPluginSettings settings,
            ISettingService settingService)
        {
            
            _productService = productService;
            _settings = settings;
            _settingService = settingService;
        }
        public async Task<IActionResult> Admin()
        {
            var a = await _productService.GetVendors();
            return View("~/Plugins/Misc.VPAPlugin/Views/Admin.cshtml", a);
        }
        public async Task<IActionResult> CheckChange(string id)
        {
            await _productService.CheckChange(id);
            return Ok();
        }

        public async Task<IActionResult> CheckProductChange(string id)
        {
            await _productService.CheckProductChange(id);
            return Ok();
        }
        public async Task<IActionResult> ChangeConfiguration()
        {
            _settings.IsNewVendorsAdminApproveNeeded = !_settings.IsNewVendorsAdminApproveNeeded;
            await _settingService.SaveSetting(_settings);
            return new JsonResult(_settings.IsNewVendorsAdminApproveNeeded);
        }
    }
}
