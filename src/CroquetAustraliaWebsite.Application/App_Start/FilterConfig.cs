using System.Web.Mvc;

namespace CroquetAustraliaWebsite.Application
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // The default 'filters.Add(new HandleErrorAttribute());' is not so ELMAH can catch all errors.

            filters.Add(new RequireHttpsAttribute());
        }
    }
}
