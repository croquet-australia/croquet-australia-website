using System;
using System.Web;
using Casper.Domain.Features.BlogPosts;
using CroquetAustraliaWebsite.Library.Content;

namespace CroquetAustraliaWebsite.Application.App.home
{
    public class BlogPostViewModel
    {
        private readonly BlogPost _blogPost;
        private readonly Lazy<IHtmlString> _contentFactory;

        public BlogPostViewModel(BlogPost blogPost, IMarkdownTransformer markdownTransformer)
        {
            _blogPost = blogPost;
            _contentFactory = new Lazy<IHtmlString>(() => markdownTransformer.MarkdownToHtml(_blogPost.Content));
        }

        public string Title { get { return _blogPost.Title; } }
        public IHtmlString Content { get { return _contentFactory.Value; } }
        public DateTimeOffset Published { get { return _blogPost.Published; } }
    }
}