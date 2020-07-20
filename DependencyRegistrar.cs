using Autofac;
using Grand.Core;
using Grand.Core.Configuration;
using Grand.Core.Infrastructure;
using Grand.Core.Infrastructure.DependencyManagement;
using Grand.Plugin.Misc.VPAPlugin.Controllers;
using Grand.Plugin.Misc.VPAPlugin.Services;
using Grand.Services.Queries.Models.Catalog;
using Grand.Web.Areas.Admin.Interfaces;
using Grand.Web.Features.Models.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Plugin.Misc.VPAPlugin
{
    public class DependencyRegistrar:IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder,GrandConfig config)
        {
            builder.RegisterType<VPAPlugin>().InstancePerLifetimeScope();
           // builder.RegisterType<VendorService>().As<IVendorService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductServiceOver>().As<Grand.Services.Catalog.IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<GetCategoryHandlerOver>().As<IRequestHandler<GetCategory, Web.Models.Catalog.CategoryModel>>().InstancePerLifetimeScope();
            builder.RegisterType<GetSearchProductsQueryHandlerOver>().As<IRequestHandler<GetSearchProductsQuery, (IPagedList<Core.Domain.Catalog.Product>, IList<string>)>>().InstancePerLifetimeScope();
            //builder.RegisterType<CreateProductModelServiceOver>().As<IProductViewModelService>().InstancePerLifetimeScope();
        }

        public int Order {
            get { return 1; }
        }
    }
}
