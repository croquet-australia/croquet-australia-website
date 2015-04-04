using System.Web.WebPages;
using CroquetAustraliaWebsite.Application.App.Infrastructure;

namespace CroquetAustraliaWebsite.Application.Layouts.Shared
{
    public static class SharedLayout
    {
        public static void Render(WebPageBase webPage, IViewModel model)
        {
            webPage.Response.Write(webPage.RenderPage("~/Layouts/Shared/BeforeRenderBody.cshtml", new { model }));
            webPage.Response.Write(webPage.RenderBody());
            webPage.Response.Write(webPage.RenderPage("~/Layouts/Shared/AfterRenderBody.cshtml", new { model }));
            webPage.Response.Write(webPage.RenderSection("scripts", required: false));
            webPage.Response.Write(webPage.RenderPage("~/Layouts/Shared/AfterRenderScripts.cshtml", new { model }));
        }
    }
}