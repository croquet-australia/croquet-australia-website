using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Anotar.NLog;
using CroquetAustralia.Library.Settings;
using CroquetAustralia.Website.App.Infrastructure;
using Elmah;

namespace CroquetAustralia.Website
{
    public class Global : HttpApplication
    {
        private static bool _filterErrorMessages;

        protected void Application_Start(object sender, EventArgs e)
        {
            LogTo.Trace("Application_Start");

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine().DisableVbhtml());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var dependencyResolver = DependencyResolver.Current;

            dependencyResolver.GetService<AppData>().Start();
            dependencyResolver.GetService<PublishedContentProviderConfig>().RegisterProviders();

            _filterErrorMessages = new ElmahSettings().FilterErrorMessages;

            if (_filterErrorMessages)
            {
                LogTo.Info("Filtering error messages.");
            }
        }

        public void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            if (!_filterErrorMessages)
            {
                return;
            }

            var httpException = e.Exception as HttpException;

            if (httpException?.GetHttpCode() == 404)
            {
                LogTo.Warn($"Page not found '{Context.Request.Url.AbsolutePath}'.");
                e.Dismiss();
                return;
            }

            var baseException = e.Exception.GetBaseException();

            // ReSharper disable once InvertIf
            if (baseException is HttpRequestValidationException)
            {
                LogTo.WarnException("Excluded HttpRequestValidationException from ELMAH.", baseException);
                e.Dismiss();
                // ReSharper disable once RedundantJumpStatement
                return;
            }
        }
    }
}