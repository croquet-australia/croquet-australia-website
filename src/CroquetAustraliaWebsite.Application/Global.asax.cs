using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Anotar.NLog;

namespace CroquetAustraliaWebsite.Application
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            LogTo.Trace("Application_Start");

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DependencyResolver.Current.GetService<AppData>().Start();
            DependencyResolver.Current.GetService<PublishedContentProviderConfig>().RegisterProviders();
        }
    }
}