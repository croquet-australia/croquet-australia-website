using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Anotar.NLog;
using CroquetAustraliaWebsite.Application.App.account.Infrastructure;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CroquetAustraliaWebsite.Application.App.account
{
    [RoutePrefix("")]
    public class AccountController : ApplicationController
    {
        private ApplicationSignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
        }

        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationUserManager>(); }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        [Route("sign-in")]
        public ActionResult SignIn(string returnUrl)
        {
            var authenticationTypes = HttpContext.GetOwinContext().Authentication.GetExternalAuthenticationTypes().ToArray();

            return View(new SignInViewModel(returnUrl, authenticationTypes));
        }

        [Route("external-sign-in")]
        [HttpPost]
        // todo: [ValidateAntiForgeryToken]
        public ActionResult ExternalSignIn(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalSignInCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [Route("external-sign-in-callback")]
        public async Task<ActionResult> ExternalSignInCallback(string returnUrl)
        {
            LogTo.Trace("ExternalSignInCallback(returnUrl: {0})", returnUrl);

            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                LogTo.Warn("ExternalSignInCallBack(returnUrl: {0}) but AuthenticationManager.GetExternalLoginInfoAsync returned null.", returnUrl);
                return RedirectToAction("SignIn");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);

            switch (result)
            {
                case SignInStatus.Success:

                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:

                    // todo
                    throw new NotImplementedException("SignInStatus.LockedOut");
                //return View("Lockout");

                case SignInStatus.RequiresVerification:

                    // todo
                    throw new NotImplementedException("SignInStatus.RequiresVerification");
                //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });

                case SignInStatus.Failure:

                    // If the user does not yet exist in UserLoginStore then sign in status is failure. Ridiculous but true.
                    var user = await AddExternalUserToUserStore(loginInfo, returnUrl);
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToLocal(returnUrl);

                default:

                    throw new NotSupportedException(string.Format("SignInStatus '{0}' is not supported. It did not exist at time of writing code.", result));
            }
        }

        private async Task<ApplicationUser> AddExternalUserToUserStore(ExternalLoginInfo loginInfo, string returnUrl)
        {
            var user = new ApplicationUser(loginInfo.ExternalIdentity.Name, loginInfo.Email);
            var result = await UserManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                throw CreateUserException(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);

            if (!result.Succeeded)
            {
                throw CreateUserException(result);
            }

            return user;
        }

        private static Exception CreateUserException(IdentityResult result)
        {
            return new Exception(string.Format("UserManager.CreateAsync(user) failed.{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, result.Errors)));
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}