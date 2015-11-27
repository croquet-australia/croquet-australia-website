using System.Collections.Generic;
using System.Linq;
using Casper.Domain.Features.BlogPosts;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Content;

namespace CroquetAustraliaWebsite.Application.App.home
{
    public class IndexViewModel : ViewModel
    {
        public IndexViewModel(IEnumerable<BlogPost> blogPosts, IMarkdownTransformer markdownTransformer)
        {
            BlogPosts = blogPosts
                .OrderByDescending(b => b.Published)
                .Select(b => new BlogPostViewModel(b, markdownTransformer));
        }

        public IEnumerable<BlogPostViewModel> BlogPosts { get; private set; }
    }
}