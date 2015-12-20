using System.Web.Mvc;

namespace CroquetAustralia.Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // The default 'filters.Add(new HandleErrorAttribute());' is not so ELMAH can catch all errors.

            // todo: resinstate - lters.Add(new RequireHttpsAttribute());
        }
    }
}
