using System.Threading.Tasks;
using System.Web.Mvc;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Content;

namespace CroquetAustraliaWebsite.Application.App.home
{
    [RoutePrefix("")]
    public class HomeController : ApplicationController
    {
        private readonly IPublishedBlogPostRepository _publishedBlogPostRepository;

        public HomeController(IPublishedBlogPostRepository publishedBlogPostRepository)
        {
            _publishedBlogPostRepository = publishedBlogPostRepository;
        }

        [Route("")]
        public Task<ViewResult> Index()
        {
            var blogPosts = _publishedBlogPostRepository.GetAsync(new Pagination());
            var viewModel = new IndexViewModel(blogPosts);

            return Task.FromResult(View(viewModel));
        }
    }
}