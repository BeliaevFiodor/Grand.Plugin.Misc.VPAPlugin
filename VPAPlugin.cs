using Grand.Core.Plugins;
using Grand.Framework.Menu;
using Grand.Services.Common;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.VPAPlugin
{
    public class VPAPlugin : BasePlugin, IMiscPlugin, IAdminMenuPlugin
    {
        public override async Task Install()
        {
            await base.Install();
        }



        public override async Task Uninstall()
        {
            await base.Uninstall();
        }
        public async Task ManageSiteMap(SiteMapNode rootNode)
        {
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Bostil");
            if (pluginNode == null)
            {
                rootNode.ChildNodes.Add(new SiteMapNode() {
                    SystemName = "Bostil",
                    Title = "Bostil",
                    Visible = true,
                    IconClass = "icon-puzzle"
                });
                pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Bostil");
            }
            var menu = new SiteMapNode();
            menu.Title = "VPA plugin";
            menu.Visible = true;
            menu.ControllerName = "VPAPlugin";
            menu.ActionName = "Admin";
            menu.RouteValues = new Microsoft.AspNetCore.Routing.RouteValueDictionary() { { "area", "Admin" } };
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menu);
            else
                rootNode.ChildNodes.Add(menu);

        }
    }
}