using System;
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
using CroquetAustraliaWebsite.Library.Authentication.DAL;
using CroquetAustraliaWebsite.Library.Authentication.Domain;
using CroquetAustraliaWebsite.Library.Authentication.Identity;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.IO;
using CroquetAustraliaWebsite.Library.Settings;
using Microsoft.AspNet.Identity;
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

            // todo: research scopes. e.g. InRequestScope().

            // Bind to methods
            kernel.Bind<ContentSettings>().ToMethod(context => new ContentSettings(HttpContext.Current.Server));
            kernel.Bind<GitContentRepositorySettings>().ToMethod(context => context.Kernel.Get<ContentSettings>().Repository);
            kernel.Bind<PublishedContentRepositorySettings>().ToMethod(context => context.Kernel.Get<ContentSettings>().PubishedRepository);
            kernel.Bind<IGitRepositorySettings>().ToMethod(GitRepositorySettings);
            kernel.Bind<IBlogPostRepositorySettings>().ToMethod(BlogPostRepositorySettings);
            kernel.Bind<IPageRepositorySettings>().ToMethod(PageRepositorySettings);
            kernel.Bind<SignInManager>().ToMethod(SignInManagerFactory);
            kernel.Bind<ICommandBus>().ToMethod(CommandBusFactory);

            // Bind to classes
            kernel.Bind<IBlogPostRepository>().To<BlogPostRepository>();
            kernel.Bind<IPageRepository>().To<PageRepository>();
            kernel.Bind<IGitRepository>().To<GitRepository>();
            kernel.Bind<IEventBus>().To<EventBus>();
            kernel.Bind<IFileOperations>().To<FileOperations>();
            kernel.Bind<IGitContentRepository>().To<GitContentRepository>();
            kernel.Bind<IPublishedContentRepository>().To<PublishedContentRepository>();
            kernel.Bind<ISlugFactory>().To<SlugFactory>();
            kernel.Bind<IYamlMarkdown>().To<YamlMarkdown>();
            kernel.Bind<IClock>().To<Clock>();
            kernel.Bind<IMarkdownTransformer>().To<MarkdownTransformer>();
            kernel.Bind<IUserStore<IdentityUser, Guid>>().To<InMemoryUserStore>();
            kernel.Bind<IUserRepository>().To<InMemoryUserRepository>();
        }

        private static IBlogPostRepositorySettings BlogPostRepositorySettings(IContext context)
        {
            var contentSettings = context.Kernel.Get<ContentSettings>();
            var publishedContentRepositorySettings = context.Kernel.Get<PublishedContentRepositorySettings>();
            var options = new BlogPostRepositorySettings(new DirectoryInfo(publishedContentRepositorySettings.Directory), contentSettings.BlogDirectoryName);

            return options;
        }

        private static ICommandBus CommandBusFactory(IContext context)
        {
            var kernel = context.Kernel;
            var commandBus = new CommandBus(kernel.Get<IEventBus>());
            var blogPostRepository = kernel.Get<IBlogPostRepository>();
            var pageRepository = kernel.Get<IPageRepository>();

            Configuration.Configure(commandBus, blogPostRepository, pageRepository);

            return commandBus;
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

        private static IPageRepositorySettings PageRepositorySettings(IContext context)
        {
            var settings = context.Kernel.Get<PublishedContentRepositorySettings>();
            var options = new PageRepositorySettings(new DirectoryInfo(settings.Directory));

            return options;
        }

        private static SignInManager SignInManagerFactory(IContext context)
        {
            var kernel = context.Kernel;
            var userManager = kernel.Get<UserManager>();
            var owinContext = HttpContext.Current.GetOwinContext();
            var authentication = owinContext.Authentication;

            return new SignInManager(userManager, authentication);
        }
    }
}