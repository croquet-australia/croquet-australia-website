using System.Web;
using Anotar.NLog;
using CommonServiceLocator.NinjectAdapter.Unofficial;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.IO;
using CroquetAustraliaWebsite.Library.Settings;
using Microsoft.Practices.ServiceLocation;
using Ninject;

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

            kernel.Bind<IFileOperations>().To<FileOperations>();
            kernel.Bind<IGitContentRepository>().To<GitContentRepository>();
            kernel.Bind<IPublishedContentRepository>().To<PublishedContentRepository>();
        }
    }
}