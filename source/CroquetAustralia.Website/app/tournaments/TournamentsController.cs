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

        [Route("{year}/{discipline}/{slug}")]
        public ViewResult Tournament()
        {
            return View("tournament");
        }

        [Route("deposited")]
        public async Task<ViewResult> Deposited(Guid id)
        {
            await _webApi.PostAsync("/tournament-entry/payment-received", new {entityId = id, paymentMethod = "EFT"});
            return View("deposited");
        }

        protected override IViewModel CreateDefaultViewModel()
        {
            return CreateDefaultViewModel(false);
        }
    }
}