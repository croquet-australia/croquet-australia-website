using System.Web.Configuration;
using System.Web.Http;
using CroquetAustralia.Library.Authentication.Identity;
using CroquetAustralia.Website;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Practices.ServiceLocation;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CroquetAustralia.Website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureWebsite();
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

        private static void ConfigureWebsite()
        {
            var config = new HttpConfiguration();

            // config.MapHttpAttributeRoutes();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{id}",
                new { id = RouteParameter.Optional, controller = "values" });

            //app.UseNinjectMiddleware(NinjectWebCommon.bootstrapper.Kernel).UseNinjectWebApi(config);
        }
    }
}