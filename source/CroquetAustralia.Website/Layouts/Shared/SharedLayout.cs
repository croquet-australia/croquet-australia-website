using System.Web.WebPages;
using CroquetAustralia.Website.App.Infrastructure;

namespace CroquetAustralia.Website.Layouts.Shared
{
    public static class SharedLayout
    {
        public static void Render(WebPageBase webPage, IViewModel model)
        {
            webPage.Response.Write(webPage.RenderPage("~/Layouts/Shared/BeforeRenderBody.cshtml", new {model}));
            webPage.Response.Write(webPage.RenderBody());
            webPage.Response.Write(webPage.RenderPage("~/Layouts/Shared/AfterRenderBody.cshtml", new {model}));
            webPage.Response.Write(webPage.RenderSection("scripts", false));
            webPage.Response.Write(webPage.RenderPage("~/Layouts/Shared/AfterRenderScripts.cshtml", new {model}));
        }
    }
}