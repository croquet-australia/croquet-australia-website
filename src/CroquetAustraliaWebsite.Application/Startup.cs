using System.Web.Configuration;
using CroquetAustraliaWebsite.Application;
using CroquetAustraliaWebsite.Library.Authentication.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Practices.ServiceLocation;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace CroquetAustraliaWebsite.Application
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        private static void ConfigureAuth(IAppBuilder app)
        {
            // todo?: app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.CreatePerOwinContext(() => ServiceLocator.Current.GetInstance<UserManager>());
            app.CreatePerOwinContext(() => ServiceLocator.Current.GetInstance<SignInManager>());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/sign-in")
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = WebConfigurationManager.AppSettings["OAuth:Google:ClientId"],
                ClientSecret = WebConfigurationManager.AppSettings["OAuth:Google:ClientSecret"]
            });
        }
    }
}
