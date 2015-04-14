using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Anotar.NLog;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.Messaging;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.Infrastructure;
using CroquetAustraliaWebsite.Library.Settings;

namespace CroquetAustraliaWebsite.Application.App.admin.home
{
    [RoutePrefix("admin")]
    public class HomeController : AdminController
    {
        private readonly IGitContentRepository _gitRepository;
        private readonly IPublishedContentRepository _publishedRepository;
        private readonly ICommandBus _commandBus;
        private readonly ContentSettings _contentSettings;
        private readonly ISlugFactory _slugFactory;

        public HomeController(IGitContentRepository gitRepository, IPublishedContentRepository publishedRepository, ICommandBus commandBus, ContentSettings contentSettings, ISlugFactory slugFactory)
        {
            _gitRepository = gitRepository;
            _publishedRepository = publishedRepository;
            _commandBus = commandBus;
            _contentSettings = contentSettings;
            _slugFactory = slugFactory;
        }

        [Route("{directory?}")]
        public async Task<ViewResult> Index(string directory = @"")
        {
            return View(await IndexViewModel.CreateInstance(_gitRepository.Directory, directory));
        }

        [Route("add-news")]
        public ViewResult AddNews()
        {
            LogTo.Trace("AddNews()");

            // ReSharper disable once UseObjectOrCollectionInitializer
            var viewModel = new AddNewsViewModel();

#if DEBUG
            var now = DateTime.Now.ToString("s");

            viewModel.Title = "news - title - " + now;
            viewModel.Content = "news - content - " + now;
#endif

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
                var user = await this.GetApplicationUserAsync();
                var author = user.ToAuthor();
                var command = new PublishBlogPost(viewModel.Title, viewModel.Content, author.Clock.Now, author, _contentSettings.BlogDirectoryName, _slugFactory);

                await _commandBus.SendCommandAsync(command);

                return RedirectToAction("index");
            }

            LogTo.Debug("Model is invalid.{0}{1}", Environment.NewLine, ModelState.ErrorsAsLoggingString());
            return View(viewModel);
        }

        [Route("new-page/{directory?}")]
        public ViewResult NewPage(string directory = "")
        {
            LogTo.Trace("NewPage(directory: {0})", directory ?? "<null>");

            Argument.CannotBeNull("directory", directory);

            var viewModel = new NewPageViewModel(_gitRepository.Directory, directory);

#if DEBUG
            var now = DateTime.Now.ToString("s");

            viewModel.PageName = "page - title - " + now;
            viewModel.Content = "page - content - " + now;
#endif

            return View(viewModel);
        }

        [HttpPost]
        [Route("new-page")]
        // todo: [ValidateAntiForgeryToken]
        public ActionResult NewPage(NewPageViewModel viewModel)
        {
            LogTo.Trace("NewPage(viewModel: {0})", viewModel);

            if (ModelState.IsValid)
            {
                switch (viewModel.SubmitButton)
                {
                    case "Publish":
                        MarkdownPage.PublishPage(viewModel.FullDirectoryPath, viewModel.PageName, viewModel.Content, _gitRepository, _publishedRepository);
                        break;
                        
                    default:
                        throw new NotImplementedException(string.Format("'{0}' button has not been implemented.", viewModel.SubmitButton));
                }

                return RedirectToAction("index", new {directory = viewModel.Directory});
            }

            LogTo.Debug("Model is invalid.{0}{1}", Environment.NewLine, ModelState.ErrorsAsLoggingString());
            return View(viewModel);
        }
    }
}