using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Plugin.Misc.VPAPlugin
{
    public class RouteProvider:Grand.Framework.Mvc.Routing.IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapControllerRoute("Plugin.Misc.VPAPlugin",
                "admin",
                new { controller = "VPAPlugin", action = "Admin" });

        }
        public int Priority {
            get {
                return 10;
            }
        }
    }
}
