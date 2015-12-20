using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using CroquetAustralia.Website.App.Infrastructure;

namespace CroquetAustralia.Website.App.tournaments
{
    [RoutePrefix("tournaments")]
    public class TournamentsController : ApplicationController
    {
        [Route("2016/ac/mens-open")]
        public ViewResult Mens_AC_Open_2016()
        {
            return View("gender-open");
        }

        [Route("2016/ac/womens-open")]
        public Task<ViewResult> Womens_AC_Open_2016()
        {
            throw new NotImplementedException();
        }
    }
}