using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using Anotar.NLog;
using Casper.Core;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.Pages.Commands;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.Messaging;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.Infrastructure;
using CroquetAustraliaWebsite.Library.Repositories;
using CroquetAustraliaWebsite.Library.Settings;

namespace CroquetAustraliaWebsite.Application.App.admin.home
{
    [RoutePrefix("admin")]
    public class HomeController : AdminController
    {
        private readonly ICommandBus _commandBus;
        private readonly IApplicationPageRepository _pageRepository;
        private readonly ContentSettings _contentSettings;
        private readonly IGitContentRepository _gitRepository;
        private readonly ISlugFactory _slugFactory;

        public HomeController(ContentSettings contentSettings, ICommandBus commandBus, IApplicationPageRepository pageRepository, IGitContentRepository gitRepository, ISlugFactory slugFactory)
        {
            _contentSettings = contentSettings;
            _commandBus = commandBus;
            _pageRepository = pageRepository;
            _gitRepository = gitRepository;
            _slugFactory = slugFactory;
        }

        [Route("{directory?}")]
        public async Task<ViewResult> Index(string directory = @"")
        {
            var directories = await _pageRepository.FindPublishedDirectoriesAsync(directory);
            var pages = await _pageRepository.FindPublishedPagesAsync(directory);
            var viewModel = new IndexViewModel(directory, directories, pages);

            return View(viewModel);
        }

        [Route("add-news")]
        public ViewResult AddNews()
        {
            LogTo.Trace("AddNews()");

            // ReSharper disable once UseObjectOrCollectionInitializer
            var viewModel = new AddNewsViewModel();

            AddDebugDefaults(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("add-news")]
        // todo: [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNews(AddNewsViewModel viewModel)
        {
            LogTo.Trace("AddNews(viewModel: {0})", viewModel);

            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var user = await this.GetApplicationUserAsync();
                var author = user.ToAuthor();
                var published = user.TimeZoneInfo.ConvertTimeFromUtc(now);
                var relativeUri = string.Format("{0}/{1}/{2}", _contentSettings.BlogDirectoryName, published.DateTime.ToFolders(), _slugFactory.CreateSlug(viewModel.Title));

                var command = new PublishBlogPost(relativeUri, viewModel.Title, viewModel.Content, published, author);

                await _commandBus.SendCommandAsync(command);

                return Redirect(Urls.Admin.Index());
            }

            LogTo.Debug("Model is invalid.{0}{1}", Environment.NewLine, ModelState.ErrorsAsLoggingString());
            return View(viewModel);
        }

        [Route("add-page/{directory?}")]
        public ViewResult AddPage(string directory = "")
        {
            LogTo.Trace("AddPage(directory: {0})", directory ?? "<null>");

            Argument.CannotBeNull("directory", directory);

            var viewModel = new AddPageViewModel(_gitRepository.Directory, directory);

            AddDebugDefaults(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Route("add-page")]
        // todo: [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPage(AddPageViewModel viewModel)
        {
            LogTo.Trace("AddPage(viewModel: {0})", viewModel);

            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var user = await this.GetApplicationUserAsync();
                var author = user.ToAuthor();
                var published = user.TimeZoneInfo.ConvertTimeFromUtc(now);
                var relativeUri = string.Format("{0}/{1}", viewModel.Directory.TrimSlashes(), _slugFactory.CreateSlug(viewModel.PageName)).TrimSlashes();

                var command = new PublishPage(relativeUri, viewModel.Content.AsMarkdown().GetPageTitle(relativeUri), viewModel.Content, published, author);

                await _commandBus.SendCommandAsync(command);

                return Redirect(Urls.Admin.Index(viewModel.Directory));
            }

            LogTo.Debug("Model is invalid.{0}{1}", Environment.NewLine, ModelState.ErrorsAsLoggingString());
            return View(viewModel);
        }

        [Route("throw-exception")]
        public ActionResult ThrowException()
        {
            throw new Exception("As requested an exception has been thrown!");
        }

        [Conditional("DEBUG")]
        private static void AddDebugDefaults(AddNewsViewModel viewModel)
        {
            var now = DateTime.Now.ToString("s");

            viewModel.Title = "news - title - " + now;
            viewModel.Content = "news - content - " + now;
        }

        [Conditional("DEBUG")]
        private static void AddDebugDefaults(AddPageViewModel viewModel)
        {
            var now = DateTime.Now.ToString("s");

            viewModel.PageName = "page-name-" + now.Replace(".", "-").Replace(":", "-").Replace(" ", "-");
            viewModel.Content = "page - content - " + now;
        }
    }
}