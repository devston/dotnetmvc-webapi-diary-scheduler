using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DiaryScheduler.Web.Startup))]
namespace DiaryScheduler.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
