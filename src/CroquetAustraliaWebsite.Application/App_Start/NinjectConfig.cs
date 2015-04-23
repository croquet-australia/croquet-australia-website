using System.IO;
using System.Web;
using Anotar.NLog;
using Casper.Core;
using Casper.Data.Git.Git;
using Casper.Data.Git.Infrastructure;
using Casper.Data.Git.Repositories;
using Casper.Domain;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.Pages;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.Messaging;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.IO;
using CroquetAustraliaWebsite.Library.Repositories;
using CroquetAustraliaWebsite.Library.Settings;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Activation;

namespace CroquetAustraliaWebsite.Application
{
    internal class NinjectConfig
    {
        internal static void RegisterServices(IKernel kernel)
        {
            LogTo.Trace("RegisterServices(kernel)");

            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));

            // Bind to methods
            kernel.Bind<ContentSettings>().ToMethod(context => new ContentSettings(HttpContext.Current.Server));
            kernel.Bind<GitContentRepositorySettings>().ToMethod(context => context.Kernel.Get<ContentSettings>().Repository);
            kernel.Bind<PublishedContentRepositorySettings>().ToMethod(context => context.Kernel.Get<ContentSettings>().PubishedRepository);
            kernel.Bind<IGitRepositorySettings>().ToMethod(GitRepositorySettings);
            kernel.Bind<IBlogPostRepositorySettings>().ToMethod(BlogPostRepositorySettings);
            kernel.Bind<IPageRepositorySettings>().ToMethod(PageRepositorySettings);

            // Bind to classes.
            kernel.Bind<IBlogPostRepository>().To<ApplicationBlogPostRepository>();
            kernel.Bind<IApplicationBlogPostRepository>().To<ApplicationBlogPostRepository>();
            kernel.Bind<IPageRepository>().To<ApplicationPageRepository>();
            kernel.Bind<IApplicationPageRepository>().To<ApplicationPageRepository>();
            kernel.Bind<IGitRepository>().To<GitRepository>();
            kernel.Bind<IEventBus>().To<EventBus>();
            kernel.Bind<IFileOperations>().To<FileOperations>();
            kernel.Bind<IGitContentRepository>().To<GitContentRepository>();
            kernel.Bind<IPublishedContentRepository>().To<PublishedContentRepository>();
            kernel.Bind<ISlugFactory>().To<SlugFactory>();
            kernel.Bind<IYamlMarkdown>().To<YamlMarkdown>();
            kernel.Bind<IClock>().To<Clock>();
            kernel.Bind<IMarkdownTransformer>().To<MarkdownTransformer>();

            kernel.Bind<ICommandBus>().ToMethod(context =>
            {
                var commandBus = new CommandBus(kernel.Get<IEventBus>());
                var blogPostRepository = kernel.Get<IBlogPostRepository>();
                var pageRepository = kernel.Get<IPageRepository>();

                Configuration.Configure(commandBus, blogPostRepository, pageRepository);

                return commandBus;
            });
        }

        private static IBlogPostRepositorySettings BlogPostRepositorySettings(IContext context)
        {
            var contentSettings = context.Kernel.Get<ContentSettings>();
            var publishedContentRepositorySettings = context.Kernel.Get<PublishedContentRepositorySettings>();
            var options = new BlogPostRepositorySettings(new DirectoryInfo(publishedContentRepositorySettings.Directory), contentSettings.BlogDirectoryName);

            return options;
        }

        private static IPageRepositorySettings PageRepositorySettings(IContext context)
        {
            var settings = context.Kernel.Get<PublishedContentRepositorySettings>();
            var options = new PageRepositorySettings(new DirectoryInfo(settings.Directory));

            return options;
        }

        private static IGitRepositorySettings GitRepositorySettings(IContext context)
        {
            var settings = context.Kernel.Get<GitContentRepositorySettings>();
            var options = new GitRepositorySettings(
                new DirectoryInfo(settings.Directory),
                settings.UserName,
                settings.Password);

            return options;
        }
    }
}