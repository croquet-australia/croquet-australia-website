using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Casper.Domain.Features.Pages;

namespace CroquetAustralia.Website.Api.Pages
{
    public class PagesController : ApiController
    {
        private readonly IPageRepository _pageRepository;

        public PagesController(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public Task<IEnumerable<Page>> GetPublished_Pages(string relativeDirectory)
        {
            return _pageRepository.FindPublishedPagesAsync(relativeDirectory);
        }
    }
}