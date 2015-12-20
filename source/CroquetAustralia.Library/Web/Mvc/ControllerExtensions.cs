using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CroquetAustralia.Library.Authentication.Domain;
using CroquetAustralia.Library.Authentication.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace CroquetAustralia.Library.Web.Mvc
{
    public static class ControllerExtensions
    {
        public static SignInManager GetSignInManager(this Controller controller)
        {
            return controller.GetOwinContext().Get<SignInManager>();
        }

        public static UserManager GetUserManager(this Controller controller)
        {
            return controller.GetOwinContext().Get<UserManager>();
        }

        public static IAuthenticationManager GetAuthenticationManager(this Controller controller)
        {
            return controller.GetOwinContext().Authentication;
        }

        public static IOwinContext GetOwinContext(this Controller controller)
        {
            // Use of extension method & NullGuard ensures exception is thrown if owin context is null.
            return controller.HttpContext.GetOwinContext();
        }
        public static async Task<DomainUser> GetDomainUserAsync(this Controller controller, IUserRepository userRepository)
        {
            var user = controller.User;

            if (user == null)
            {
                throw new InvalidOperationException("Cannot get domain user because controller.User is null.");
            }

            var userId = user.Identity.GetUserId();
            var domainUser = await userRepository.GetByIdAsync(new Guid(userId));

            return domainUser;
        }
    }
}