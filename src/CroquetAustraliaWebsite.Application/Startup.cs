using System.Web.Configuration;
using CroquetAustraliaWebsite.Application;
using CroquetAustraliaWebsite.Library.Authentication;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
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
            // Configure the user manager and sign in manager to use a single instance per request
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

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
