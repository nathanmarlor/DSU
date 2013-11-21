﻿namespace dealstealunreal.com
{
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Infrastructure.Sessions.Interfaces;
    using log4net.Config;
    using Ninject;
    using Ninject.Extensions.Logging;
    using Ninject.Extensions.Logging.Log4net.Infrastructure;
    using Ninject.Web.Common;

    public class MvcApplication : NinjectHttpApplication
    {
        private const string ConfigPath = @"C:\Users\Nathan\DSU\config\dsu.log4net.config";

        private ILogger log;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                });
        }

        protected override IKernel CreateKernel()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(ConfigPath));
            this.log = new Log4NetLogger(typeof(MvcApplication));
            this.log.Info("Starting DealStealUnreal");

            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            this.log.Info("Successfully initialised dependency injection");

            Thread pruneThread = new Thread(kernel.Get<ISessionController>().PruneSessions) { IsBackground = true };
            pruneThread.Start();

            return kernel;
        }

        protected override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            base.OnApplicationStarted();
        }
    }
}