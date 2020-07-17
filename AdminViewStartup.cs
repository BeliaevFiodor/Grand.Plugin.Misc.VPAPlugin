using Grand.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Plugin.Misc.VPAPlugin
{
    public class AdminViewStartup : IGrandStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //register admin view expander
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new AdminViewLocationExpander());
            });
        }
        public void Configure(IApplicationBuilder application, IWebHostEnvironment hostEnvironment)
        {

        }
        public int Order => 102;

    }
}
