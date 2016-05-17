using System;
using System.Web.Mvc;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public abstract class ApplicationController : Controller
    {
        private readonly string _controllerName;

        protected ApplicationController()
        {
            _controllerName = GetControllerName();
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            }

            if (model == null)
            {
                model = CreateDefaultViewModel();
            }

            return base.View(GetFeatureViewName(_controllerName, viewName), masterName, model);
        }

        protected virtual IViewModel CreateDefaultViewModel()
        {
            return CreateDefaultViewModel(true);
        }

        protected virtual IViewModel CreateDefaultViewModel(bool showSidebar)
        {
            return new ViewModel(showSidebar);
        }

        private string GetControllerName()
        {
            var fullControllerName = GetType().Name;
            var controllerName = fullControllerName.Substring(0, fullControllerName.LastIndexOf("Controller", StringComparison.Ordinal)).ToLower();

            return controllerName;
        }

        protected virtual string GetFeatureViewName(string controllerName, string viewName)
        {
            return string.Format("~/App/{0}/{1}.cshtml", _controllerName, viewName);
        }
    }
}