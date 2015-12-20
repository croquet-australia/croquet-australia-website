using System.Web.Mvc;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        public AdminAuthorizeAttribute()
        {
            Roles = "Editor";
        }
    }
}