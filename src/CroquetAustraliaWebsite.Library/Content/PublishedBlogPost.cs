using System;
using System.Web;

namespace CroquetAustraliaWebsite.Library.Content
{
    public class PublishedBlogPost
    {
        public PublishedBlogPost(string title, IHtmlString content, DateTime published)
        {
            Title = title;
            Content = content;
            Published = published;
        }

        public string Title { get; private set; }
        public IHtmlString Content { get; private set; }
        public DateTime Published { get; private set; }
    }
}