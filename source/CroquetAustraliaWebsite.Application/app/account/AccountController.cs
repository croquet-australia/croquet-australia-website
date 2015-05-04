using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Anotar.NLog;
using CroquetAustraliaWebsite.Application.App.account.Infrastructure;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Authentication.Domain;
using CroquetAustraliaWebsite.Library.Authentication.Identity;
using CroquetAustraliaWebsite.Library.Infrastructure;
using CroquetAustraliaWebsite.Library.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CroquetAustraliaWebsite.Application.App.account
{
    [RequireHttps]
    [RoutePrefix("")]
    public class AccountController : ApplicationController
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("external-sign-in")]
        [HttpPost]
        // todo: [ValidateAntiForgeryToken]
        public ActionResult ExternalSignIn(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalSignInCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        [Route("external-sign-in-callback")]
        public async Task<ActionResult> ExternalSignInCallback(string returnUrl)
        {
            LogTo.Trace("ExternalSignInCallback(returnUrl: {0})", returnUrl);

            var loginInfo = await GetExternalLoginInfoAsync();
            await TryRegisterExternalLogin(loginInfo.Login, loginInfo.Email);
            var signInStatus = await this.GetSignInManager().ExternalSignInAsync(loginInfo, false);

            LogTo.Debug("ExternalSignInCallback(returnUrl: {0}) - signInStatus: {1}", returnUrl, signInStatus);

            switch (signInStatus)
            {
                case SignInStatus.Success:
                    return HandleExternalSignInSuccess(returnUrl);

                case SignInStatus.LockedOut:
                    return HandleExternalSignInLockedOut();

                case SignInStatus.RequiresVerification:
                    return HandleExternalSignInRequiresVerification();

                case SignInStatus.Failure:
                    return HandleExternalSignInFailureAsync();

                default:
                    throw new NotSupportedException(string.Format("SignInStatus '{0}' is not supported. It did not exist at time of writing code.", signInStatus));
            }
        }

        [Route("sign-in")]
        public ActionResult SignIn(string returnUrl)
        {
            var authenticationTypes = HttpContext.GetOwinContext().Authentication.GetExternalAuthenticationTypes().ToArray();

            return View(new SignInViewModel(returnUrl, authenticationTypes));
        }

        private async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            var loginInfo = await this.GetAuthenticationManager().GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                throw new Exception("Expected AuthenticationManager.GetExternalLoginInfoAsync() to return a value, not null.");
            }

            return loginInfo;
        }

        private ActionResult HandleExternalSignInFailureAsync()
        {
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Sign in failure.");
        }

        private static ActionResult HandleExternalSignInLockedOut()
        {
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Account has been locked out.");
        }

        private static ActionResult HandleExternalSignInRequiresVerification()
        {
            throw new NotSupportedException();
        }

        private ActionResult HandleExternalSignInSuccess(string returnUrl)
        {
            return RedirectToLocal(returnUrl);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private async Task TryRegisterExternalLogin(UserLoginInfo login, string email)
        {
            LogTo.Trace("TryRegisterExternalLogin(login: {0})", login.ToLogString());

            var userManager = this.GetUserManager();
            var isRegistered = await userManager.IsRegisteredAsync(login);

            if (isRegistered)
            {
                LogTo.Debug("External login '{0}' is registered.", email);
                return;
            }

            var domainUser = await _userRepository.FindByEmailAsync(email);
            var isDomainUser = domainUser != null;

            if (!isDomainUser)
            {
                LogTo.Debug("External login '{0}' is not a recognized user.", email);
                return;
            }

            var identityUser = new IdentityUser(domainUser);
            await userManager.CreateAsync(identityUser);
            await userManager.AddLoginAsync(identityUser.Id, login);

            LogTo.Debug("Registered external login '{0}'.", email);
        }
    }
}