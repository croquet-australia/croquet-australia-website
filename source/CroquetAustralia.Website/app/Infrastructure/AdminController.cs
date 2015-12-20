using System.Web.Mvc;

namespace CroquetAustralia.Website.App.Infrastructure
{
    [AdminAuthorize]
    [RequireHttps]
    public abstract class AdminController : ApplicationController
    {
        protected override IViewModel CreateDefaultViewModel()
        {
            return new AdminViewModel();
        }

        protected override string GetFeatureViewName(string controllerName, string viewName)
        {
            return string.Format("~/App/admin/{0}/{1}.cshtml", controllerName, viewName);
        }
    }
}