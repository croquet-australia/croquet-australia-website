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
            return View("gender-open");
        }

        [Route("2016/ac/womens-open")]
        public ViewResult Womens_AC_Open_2016()
        {
            return View("gender-open");
        }

        [Route("deposited")]
        public async Task<ViewResult> Deposited(Guid id)
        {
            await _webApi.PostAsync("/tournament-entry/payment-received", new {entityId = id, paymentMethod = "EFT"});
            return View("deposited");
        }
    }
}