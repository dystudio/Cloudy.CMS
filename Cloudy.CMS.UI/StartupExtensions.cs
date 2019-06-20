﻿using Microsoft.Extensions.DependencyInjection;
using Poetry;
using Cloudy.CMS;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentControllerSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.PropertyMappingSupport;
using Cloudy.CMS.Core;
using Cloudy.CMS.Core.ContentSupport.RepositorySupport;
using Cloudy.CMS.Mvc.Routing;
using Cloudy.CMS.Reflection;
using Cloudy.CMS.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cloudy.CMS.UI.NaggingSupport;
using Microsoft.AspNetCore.Routing;
using Cloudy.CMS.AspNetCore.ContentControllerSupport;
using Poetry.AspNetCore;
using Poetry.UI.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Poetry.InitializerSupport;
using Microsoft.Extensions.FileProviders;
using Poetry.UI.AspNetCore.AuthorizationSupport;
using Poetry.UI.AspNetCore.PortalSupport;
using Poetry.UI.AspNetCore.ApiSupport;

namespace Cloudy.CMS.UI
{
    public static class StartupExtensions
    {
        public static CloudyConfigurator AddCloudyAdmin(this CloudyConfigurator configurator)
        {
            configurator.AddComponent<CloudyAdminComponent>();

            return configurator;
        }

        public static void AddContentRoute(this IRouteBuilder routes)
        {
            routes.Routes.Add(new ContentRoute(routes.DefaultHandler, routes.ApplicationBuilder.ApplicationServices.GetRequiredService<IContentRouter>(), routes.ApplicationBuilder.ApplicationServices.GetRequiredService<IContentTypeProvider>(), routes.ApplicationBuilder.ApplicationServices.GetRequiredService<IContentControllerFinder>()));
        }

        public static void UseCloudyAdmin(this IApplicationBuilder app, Action<CloudyAdminConfigurator> configure)
        {
            var options = new CloudyAdminOptions();

            configure(new CloudyAdminConfigurator(options));

            app.UseMiddleware<MainPageMiddleware>();
            app.UseMiddleware<ApiMiddleware>();

            app.Map(new PathString(options.BasePath), branch =>
            {
                branch.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly()),
                });
            });
        }
    }
}