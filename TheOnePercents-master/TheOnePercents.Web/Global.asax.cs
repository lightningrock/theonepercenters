using System;
using Ninject;
using Ninject.Web.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TheOnePercents.Infrastructure;

namespace TheOnePercents.Web
{
    public class MvcApplication : NinjectHttpApplication
    {
        //protected void Application_Start()
        //{
           
        //}

        protected override void OnApplicationStarted()
        {
            // Clears all previously registered view engines.
            ViewEngines.Engines.Clear();

            // Registers the Razor view engine.
            ViewEngines.Engines.Add(new RazorViewEngine());


            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected override void OnApplicationStopped()
        {
            base.OnApplicationStopped();
        }

        static IKernel _container;

        public static IKernel Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new StandardKernel(new InjectControllerFactory.TopsDependencies());
                }
                return _container;
            }
        }

        protected override IKernel CreateKernel()
        {
            //return Container;
            return new StandardKernel(new InjectControllerFactory.TopsDependencies());
        }
    }
}