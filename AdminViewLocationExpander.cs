using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grand.Plugin.Misc.VPAPlugin
{
    public class AdminViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue("Admin", out _))
            {
                viewLocations = new[] {
                $"/Plugins/Misc.VPAPlugin/Views/{{1}}/{{0}}.cshtml",
                $"/Plugins/Misc.VPAPlugin/Views/Shared/{{0}}.cshtml",
                }
                .Concat(viewLocations);
            }
            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //add view context only for admin
            if (context.AreaName?.Equals("Index") ?? false)
                context.Values["Admin"] = "Y";

            return;
        }
    }
}
