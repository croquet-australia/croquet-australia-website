using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Anotar.NLog;
using CroquetAustraliaWebsite.Library.Authentication;
using Microsoft.AspNet.Identity.Owin;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public static class ControllerExtensions
    {
        public static async Task<ApplicationUser> GetApplicationUserAsync(this Controller controller)
        {
            var user = controller.User;

            if (user == null)
            {
                throw new InvalidOperationException("Cannot get application user before controller.User is null.");
            }

            var applicationUser = await controller.GetUserManager().FindByNameAsync(user.Identity.Name);

            if (applicationUser == null)
            {
                throw new Exception(string.Format("Cannot find application user by name '{0}'.", user.Identity.Name));
            }

            return applicationUser;
        }

        public static ApplicationUserManager GetUserManager(this Controller controller)
        {
            return controller.HttpContext.GetOwinContext().Get<ApplicationUserManager>();
        }
    }
}