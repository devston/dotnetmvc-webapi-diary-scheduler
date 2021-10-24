using Autofac;
using Autofac.Integration.Mvc;
using DiaryScheduler.Data.Data;
using DiaryScheduler.Data.Models;
using DiaryScheduler.DependencyResolution;
using DiaryScheduler.Web.Common.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DiaryScheduler.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Set up DI.
            var builder = IoCBootstrapper.SetUpBuilder();

            builder.RegisterType<UserStore<ApplicationUser>>()
                .As<IUserStore<ApplicationUser>>()
                .InstancePerRequest();

            builder.RegisterType<ApplicationDbContext>()
                .InstancePerRequest();

            builder.RegisterType<ViewModelMapper>()
                .InstancePerRequest();

            // Register the MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register any model binders.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // Set the dependency resolver to be autofac.
            var container = builder.Build();
            IoCBootstrapper.SetIoCContainer(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// The application shut down method.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected void Application_End(object sender, EventArgs e)
        {
            // Dispose of the IoC container.
            IoCBootstrapper.DisposeContainer();
        }
    }
}
