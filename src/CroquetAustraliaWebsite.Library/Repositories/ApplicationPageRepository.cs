using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Data.Git.Repositories;

namespace CroquetAustraliaWebsite.Library.Repositories
{
    public class ApplicationPageRepository : PageRepository, IApplicationPageRepository
    {
        public ApplicationPageRepository(IPageRepositorySettings settings, IBlogPostRepositorySettings blogPostRepositorySettings, IGitRepository gitRepository, IYamlMarkdown yamlMarkdown)
            : base(settings, blogPostRepositorySettings, gitRepository, yamlMarkdown)
        {
        }
    }
}
