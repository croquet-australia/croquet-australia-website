using System.Web.Mvc;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            Roles = "Editor";
        }
    }
}