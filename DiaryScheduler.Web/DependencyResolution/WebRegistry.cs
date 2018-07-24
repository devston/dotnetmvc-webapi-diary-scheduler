using DiaryScheduler.Authentication.Data;
using DiaryScheduler.Authentication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using StructureMap;
using System.Data.Entity;
using System.Web;

namespace DiaryScheduler.Web.DependencyResolution
{
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<IUserStore<ApplicationUser>>().Use<UserStore<ApplicationUser>>();
            For<DbContext>().Use(() => new ApplicationDbContext());
            For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);
        }
    }
}