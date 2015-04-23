using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Data.Git.Repositories;

namespace CroquetAustraliaWebsite.Library.Repositories
{
    public class ApplicationBlogPostRepository : BlogPostRepository, IApplicationBlogPostRepository
    {
        public ApplicationBlogPostRepository(IBlogPostRepositorySettings settings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
            : base(settings, gitRepository, yamlMarkdown)
        {
        }
    }
}
