using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;

namespace CroquetAustralia.Library.Web.Mvc
{
    public static class ControllerExtensions
    {
        public static IOwinContext GetOwinContext(this Controller controller)
        {
            // Use of extension method & NullGuard ensures exception is thrown if owin context is null.
            return controller.HttpContext.GetOwinContext();
        }
    }
}