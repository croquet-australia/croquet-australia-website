using System.Collections.Generic;
using System.Linq;
using Casper.Domain.Features.BlogPosts;
using CroquetAustralia.Library.Content;
using CroquetAustralia.Website.App.Infrastructure;

namespace CroquetAustralia.Website.App.home
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