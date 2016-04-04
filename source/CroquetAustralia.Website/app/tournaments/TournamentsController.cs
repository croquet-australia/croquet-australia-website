using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using CroquetAustralia.Website.App.Infrastructure;

namespace CroquetAustralia.Website.App.tournaments
{
    [RoutePrefix("tournaments")]
    public class TournamentsController : ApplicationController
    {
        private readonly WebApi _webApi;

        public TournamentsController(WebApi webApi)
        {
            _webApi = webApi;
        }

        [Route("2016/ac/mens-open")]
        public ViewResult Mens_AC_Open_2016()
        {
            return View("tournament");
        }

        [Route("2016/ac/womens-open")]
        public ViewResult Womens_AC_Open_2016()
        {
            return View("tournament");
        }

        [Route("2016/gc/open-doubles")]
        public ViewResult GC_Open_Doubles_2016()
        {
            return View("tournament");
        }

        [Route("2016/gc/open-singles")]
        public ViewResult GC_Open_Singles_2016()
        {
            return View("tournament");
        }

        [Route("deposited")]
        public async Task<ViewResult> Deposited(Guid id)
        {
            await _webApi.PostAsync("/tournament-entry/payment-received", new {entityId = id, paymentMethod = "EFT"});
            return View("deposited");
        }
    }
}