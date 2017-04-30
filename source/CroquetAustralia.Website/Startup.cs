using System.Web.Http;
using CroquetAustralia.Website;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CroquetAustralia.Website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureWebsite();
        }

        private static void ConfigureWebsite()
        {
            var config = new HttpConfiguration();

            // config.MapHttpAttributeRoutes();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{id}",
                new {id = RouteParameter.Optional, controller = "values"});

            //app.UseNinjectMiddleware(NinjectWebCommon.bootstrapper.Kernel).UseNinjectWebApi(config);
        }
    }
}