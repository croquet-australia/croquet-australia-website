using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Content;

namespace CroquetAustraliaWebsite.Application.App.home
{
    public class IndexViewModel : ViewModel
    {
        public IndexViewModel(Task<IEnumerable<PublishedBlogPost>> blogPosts)
        {
            BlogPosts = blogPosts.Result.ToArray();
        }

        public PublishedBlogPost[] BlogPosts { get; private set; }
    }
}