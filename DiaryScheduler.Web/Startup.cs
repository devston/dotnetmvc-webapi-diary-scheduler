using DiaryScheduler.DependencyResolution;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DiaryScheduler.Web.Startup))]
namespace DiaryScheduler.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Set the dependency resolver to be autofac.
            var container = IoCBootstrapper.GetIoCContainer();
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app);
        }
    }
}
