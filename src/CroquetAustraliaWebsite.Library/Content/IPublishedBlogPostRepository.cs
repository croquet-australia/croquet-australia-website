using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustraliaWebsite.Library.Content
{
    // todo: move to casper
    public interface IPublishedBlogPostRepository
    {
        Task<IEnumerable<PublishedBlogPost>> GetAsync(IPagination pagination);
    }
}
