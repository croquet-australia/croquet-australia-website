using System.IO;
using System.Web;
using Anotar.NLog;
using Casper.Data.Git.Git;
using Casper.Data.Git.Repositories;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.Messaging;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.IO;
using CroquetAustraliaWebsite.Library.Settings;
using LibGit2Sharp;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Activation;
using Configuration = Casper.Domain.Configuration;

namespace CroquetAustraliaWebsite.Application
{
    internal class NinjectConfig
    {
        internal static void RegisterServices(IKernel kernel)
        {
            LogTo.Trace("RegisterServices(kernel)");

            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));


            kernel.Bind<ContentSettings>().ToMethod(context => new ContentSettings(HttpContext.Current.Server));
            kernel.Bind<GitContentRepositorySettings>().ToMethod(context => context.Kernel.Get<ContentSettings>().Repository);
            kernel.Bind<PublishedContentRepositorySettings>().ToMethod(context => context.Kernel.Get<ContentSettings>().PubishedRepository);
            kernel.Bind<IGitRepositoryOptions>().ToMethod(GitRepositoryOptionsFactory);
            kernel.Bind<IPublishedBlogPostRepository>().ToMethod(context => new PublishedBlogPostRepository(Path.Combine(context.Kernel.Get<PublishedContentRepositorySettings>().Directory, context.Kernel.Get<ContentSettings>().BlogDirectoryName)));

            kernel.Bind<IGitRepository>().To<GitRepository>();
            kernel.Bind<IEventBus>().To<EventBus>();
            kernel.Bind<IFileOperations>().To<FileOperations>();
            kernel.Bind<IGitContentRepository>().To<GitContentRepository>();
            kernel.Bind<IPublishedContentRepository>().To<PublishedContentRepository>();
            kernel.Bind<ISlugFactory>().To<SlugFactory>();
            kernel.Bind<IBlogPostRepository>().To<BlogPostRepository>();

            kernel.Bind<ICommandBus>().ToMethod(context =>
            {
                var commandBus = new CommandBus(kernel.Get<IEventBus>());
                var blogPostRepository = kernel.Get<IBlogPostRepository>();

                Configuration.Configure(commandBus, blogPostRepository);

                return commandBus;
            });

        }

        private static IGitRepositoryOptions GitRepositoryOptionsFactory(IContext context)
        {
            var settings = context.Kernel.Get<GitContentRepositorySettings>();
            var options = new GitRepositoryOptions(
                new DirectoryInfo(settings.Directory),
                settings.UserName,
                settings.Password);

            return options;
        }
    }
}