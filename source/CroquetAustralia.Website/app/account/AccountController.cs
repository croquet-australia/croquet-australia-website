using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Anotar.NLog;
using CroquetAustralia.Library.Authentication.Domain;
using CroquetAustralia.Library.Authentication.Identity;
using CroquetAustralia.Library.Infrastructure;
using CroquetAustralia.Library.Web.Mvc;
using CroquetAustralia.Website.App.account.Infrastructure;
using CroquetAustralia.Website.App.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CroquetAustralia.Website.App.account
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
            LogTo.Trace($"ExternalSignIn(provider: {provider}, returnUrl: {returnUrl})");

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalSignInCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        [Route("external-sign-in-callback")]
        public async Task<ActionResult> ExternalSignInCallback(string returnUrl)
        {
            LogTo.Trace($"ExternalSignInCallback(returnUrl: {returnUrl})");

            var loginInfo = await GetExternalLoginInfoAsync();
            await TryRegisterExternalLogin(loginInfo.Login, loginInfo.Email);
            var signInStatus = await this.GetSignInManager().ExternalSignInAsync(loginInfo, false);

            LogTo.Debug($"ExternalSignInCallback(returnUrl: {returnUrl}) - signInStatus: {signInStatus}");

            switch (signInStatus)
            {
                case SignInStatus.Success:
                    return HandleExternalSignInSuccess(returnUrl);

                case SignInStatus.LockedOut:
                    return HandleExternalSignInLockedOut();

                case SignInStatus.RequiresVerification:
                    return HandleExternalSignInRequiresVerification();

                case SignInStatus.Failure:
                    return HandleExternalSignInFailure();

                default:
                    throw new NotSupportedException(string.Format("SignInStatus '{0}' is not supported. It did not exist at time of writing code.", signInStatus));
            }
        }

        [Route("sign-in")]
        public ActionResult SignIn(string returnUrl)
        {
            LogTo.Trace($"SignIn(returnUrl: {returnUrl})");

            var authenticationTypes = HttpContext.GetOwinContext().Authentication.GetExternalAuthenticationTypes().ToArray();

            return View(new SignInViewModel(returnUrl, authenticationTypes));
        }

        private async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            LogTo.Trace("GetExternalLoginInfoAsync()");

            var loginInfo = await this.GetAuthenticationManager().GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                throw new Exception("Expected AuthenticationManager.GetExternalLoginInfoAsync() to return a value, not null.");
            }

            return loginInfo;
        }

        private static ActionResult HandleExternalSignInFailure()
        {
            LogTo.Trace($"{nameof(HandleExternalSignInFailure)}()");

            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Sign in failure.");
        }

        private static ActionResult HandleExternalSignInLockedOut()
        {
            LogTo.Trace($"{nameof(HandleExternalSignInLockedOut)}()");

            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Account has been locked out.");
        }

        private static ActionResult HandleExternalSignInRequiresVerification()
        {
            LogTo.Trace($"{nameof(HandleExternalSignInRequiresVerification)}()");

            throw new NotSupportedException();
        }

        private ActionResult HandleExternalSignInSuccess(string returnUrl)
        {
            LogTo.Trace($"HandleExternalSignInSuccess(string '{returnUrl}')");

            return RedirectToLocal(returnUrl);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            LogTo.Trace($"RedirectToLocal(string '{returnUrl}')");

            if (Url.IsLocalUrl(returnUrl))
            {
                LogTo.Trace($"RedirectToLocal(string '{returnUrl}') is redirecting to local");
                return Redirect(returnUrl);
            }

            LogTo.Trace($"RedirectToLocal(string '{returnUrl}') is redirecting to tournaments");
            // ReSharper disable once Mvc.ActionNotResolved
            return RedirectToAction("Index", "Tournaments");
        }

        private async Task TryRegisterExternalLogin(UserLoginInfo login, string email)
        {
            LogTo.Trace($"TryRegisterExternalLogin(login: {login.ToLogString()})");

            var userManager = this.GetUserManager();
            var isRegistered = await userManager.IsRegisteredAsync(login);

            if (isRegistered)
            {
                LogTo.Debug($"External login '{email}' is registered.");
                return;
            }

            var domainUser = await _userRepository.FindByEmailAsync(email);
            var isDomainUser = domainUser != null;

            if (!isDomainUser)
            {
                LogTo.Debug($"External login '{email}' is not a recognized user.");
                return;
            }

            var identityUser = new IdentityUser(domainUser);
            await userManager.CreateAsync(identityUser);
            await userManager.AddLoginAsync(identityUser.Id, login);

            LogTo.Debug($"Registered external login '{email}'.");
        }
    }
}