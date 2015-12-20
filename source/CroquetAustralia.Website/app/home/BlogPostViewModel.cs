using System;
using System.Web;
using Anotar.NLog;
using Casper.Domain.Features.BlogPosts;
using CroquetAustralia.Library.Content;

namespace CroquetAustralia.Website.App.home
{
    public class BlogPostViewModel
    {
        private readonly BlogPost _blogPost;
        private readonly Lazy<IHtmlString> _contentFactory;

        public BlogPostViewModel(BlogPost blogPost, IMarkdownTransformer markdownTransformer)
        {
            _blogPost = blogPost;
            _contentFactory = new Lazy<IHtmlString>(() => MarkdownToHtml(markdownTransformer));
        }

        public string Title => _blogPost.Title;

        public IHtmlString Content => _contentFactory.Value;

        public DateTimeOffset Published => _blogPost.Published;

        private IHtmlString MarkdownToHtml(IMarkdownTransformer markdownTransformer)
        {
            try
            {
                return markdownTransformer.MarkdownToHtml(_blogPost.Content);
            }
            catch (Exception innerException)
            {
                var exception = new Exception($"Could not convert content of blog post '{Title}' to HTML.",
                    innerException);

                exception.Data.Add("BlogPost.Title", _blogPost.Title);
                exception.Data.Add("BlogPost.Content", _blogPost.Content);
                exception.Data.Add("BlogPost.Published", _blogPost.Published);
                exception.Data.Add("BlogPost.RelativeUri", _blogPost.RelativeUri);

                LogTo.ErrorException(exception.Message, exception);

                return new HtmlString("<p>Content currently not available.<p>");
            }
        }
    }
}