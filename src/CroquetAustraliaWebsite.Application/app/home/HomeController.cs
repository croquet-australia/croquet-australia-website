using System.Threading.Tasks;
using System.Web.Mvc;
using Casper.Domain.Infrastructure;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.Repositories;

namespace CroquetAustraliaWebsite.Application.App.home
{
    [RoutePrefix("")]
    public class HomeController : ApplicationController
    {
        private readonly IApplicationBlogPostRepository _blogPostRepository;
        private readonly IMarkdownTransformer _markdownTransformer;

        public HomeController(IApplicationBlogPostRepository blogPostRepository, IMarkdownTransformer markdownTransformer)
        {
            _blogPostRepository = blogPostRepository;
            _markdownTransformer = markdownTransformer;
        }

        [Route("")]
        public async Task<ViewResult> Index()
        {
            var blogPosts = await _blogPostRepository.FindPublishedBlogPostsAsync(new Pagination(1, 10));
            var viewModel = new IndexViewModel(blogPosts, _markdownTransformer);

            return View(viewModel);
        }
    }
}