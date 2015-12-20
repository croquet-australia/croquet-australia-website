using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Anotar.NLog;
using Casper.Core;
using Casper.Domain.Features.BlogPosts;
using Casper.Domain.Features.BlogPosts.Commands;
using Casper.Domain.Features.Pages;
using Casper.Domain.Features.Pages.Commands;
using Casper.Domain.Infrastructure;
using Casper.Domain.Infrastructure.Messaging;
using CroquetAustralia.Library.Authentication.Domain;
using CroquetAustralia.Library.Content;
using CroquetAustralia.Library.Infrastructure;
using CroquetAustralia.Library.Settings;
using CroquetAustralia.Library.Web.Mvc;
using CroquetAustralia.Website.App.Infrastructure;
using OpenMagic.Extensions;

namespace CroquetAustralia.Website.App.admin.home
{
    [RoutePrefix("admin")]
    public class HomeController : AdminController
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICommandBus _commandBus;
        private readonly ContentSettings _contentSettings;
        private readonly IGitContentRepository _gitRepository;
        private readonly IPageRepository _pageRepository;
        private readonly ISlugFactory _slugFactory;
        private readonly IUserRepository _userRepository;

        public HomeController(ContentSettings contentSettings, ICommandBus commandBus, IPageRepository pageRepository,
            IGitContentRepository gitRepository, IUserRepository userRepository, ISlugFactory slugFactory,
            IBlogPostRepository blogPostRepository)
        {
            _contentSettings = contentSettings;
            _commandBus = commandBus;
            _pageRepository = pageRepository;
            _gitRepository = gitRepository;
            _slugFactory = slugFactory;
            _blogPostRepository = blogPostRepository;
            _userRepository = userRepository;
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

        [HttpPost, ValidateInput(false)]
        [Route("add-news")]
        // todo: [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNews(AddNewsViewModel viewModel)
        {
            LogTo.Trace("AddNews(viewModel: {0})", viewModel);

            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var user = await this.GetDomainUserAsync(_userRepository);
                var author = user.ToAuthor();
                var published = user.TimeZoneInfo.ConvertTimeFromUtc(now);
                var relativeUri = string.Format("{0}/{1}/{2}", _contentSettings.BlogDirectoryName,
                    published.DateTime.ToFolders(), _slugFactory.CreateSlug(viewModel.Title));

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

        [HttpPost, ValidateInput(false)]
        [Route("add-page")]
        // todo: [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPage(AddPageViewModel viewModel)
        {
            LogTo.Trace("AddPage(viewModel: {0})", viewModel);

            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var user = await this.GetDomainUserAsync(_userRepository);
                var author = user.ToAuthor();
                var published = user.TimeZoneInfo.ConvertTimeFromUtc(now);
                var relativeUri =
                    string.Format("{0}/{1}", viewModel.Directory.TrimSlashes(),
                        _slugFactory.CreateSlug(viewModel.PageName)).TrimSlashes();

                var command = new PublishPage(relativeUri, viewModel.Content.AsMarkdown().GetPageTitle(relativeUri),
                    viewModel.Content, published, author);

                await _commandBus.SendCommandAsync(command);

                return Redirect(Urls.Admin.Index(viewModel.Directory));
            }

            LogTo.Debug("Model is invalid.{0}{1}", Environment.NewLine, ModelState.ErrorsAsLoggingString());
            return View(viewModel);
        }

        [Route("edit-news/{relativeUri?}")]
        public async Task<ViewResult> EditNews(string relativeUri)
        {
            LogTo.Trace("EditNews(relativeUri: {0})", relativeUri);

            var blogPost = await GetPublishedBlogPostAsync(relativeUri);
            var viewModel = new EditNewsViewModel(blogPost);

            return View(viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [Route("edit-news")]
        // todo: [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditNews(EditNewsViewModel viewModel)
        {
            LogTo.Trace("EditNews(viewModel: {0})", viewModel);

            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var user = await this.GetDomainUserAsync(_userRepository);
                var author = user.ToAuthor();
                var published = user.TimeZoneInfo.ConvertTimeFromUtc(now);
                var relativeUri = viewModel.RelativeUri;

                var command = new PublishBlogPost(relativeUri, viewModel.Title, viewModel.Content, published, author);

                await _commandBus.SendCommandAsync(command);

                return Redirect(Urls.Admin.Index(_contentSettings.BlogDirectoryName));
            }

            LogTo.Debug("Model is invalid.{0}{1}", Environment.NewLine, ModelState.ErrorsAsLoggingString());
            return View(viewModel);
        }

        private async Task<BlogPost> GetPublishedBlogPostAsync(string relativeUri)
        {
            // todo: this is a quick hack because this project is likely replaced with ImpressiveMark.
            var blogPosts = await _blogPostRepository.FindPublishedBlogPostsAsync(new Pagination(1, int.MaxValue));
            var blogPost = blogPosts.SingleOrDefault(b => b.RelativeUri == relativeUri);

            if (blogPost != null)
            {
                return blogPost;
            }

            throw new Exception($"Cannot find blog post '{relativeUri}'.");
        }

        [Route("edit-page/{relativeUri?}")]
        public async Task<ViewResult> EditPage(string relativeUri)
        {
            LogTo.Trace("EditPage(relativeUri: {0})", relativeUri);

            var page = await _pageRepository.GetPublishedPageAsync(relativeUri);
            var viewModel = new EditPageViewModel(page);

            return View(viewModel);
        }

        [HttpPost, ValidateInput(false)]
        [Route("edit-page")]
        // todo: [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPage(EditPageViewModel viewModel)
        {
            LogTo.Trace("EditPage(viewModel: {0})", viewModel);

            if (ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var user = await this.GetDomainUserAsync(_userRepository);
                var author = user.ToAuthor();
                var published = user.TimeZoneInfo.ConvertTimeFromUtc(now);
                var relativeUri = viewModel.RelativeUri;

                var command = new PublishPage(relativeUri, viewModel.Content.AsMarkdown().GetPageTitle(relativeUri),
                    viewModel.Content, published, author);

                await _commandBus.SendCommandAsync(command);

                return Redirect(Urls.Admin.Index(viewModel.RelativeUri.TextBeforeLast("/")));
            }

            LogTo.Debug("Model is invalid.{0}{1}", Environment.NewLine, ModelState.ErrorsAsLoggingString());
            return View(viewModel);
        }

        [Route("{directory?}")]
        public async Task<ViewResult> Index(string directory = @"")
        {
            var isNewsDirectory = directory.StartsWith(_contentSettings.BlogDirectoryName);

            return isNewsDirectory
                ? await NewsView(directory)
                : await PagesView(directory);
        }

        private async Task<ViewResult> NewsView(string blogDirectoryName)
        {
            var blogPosts = await _blogPostRepository.FindPublishedBlogPostsAsync(new Pagination(1, int.MaxValue));
            var viewModel = new NewsViewModel(blogDirectoryName, blogPosts);

            return View("News", viewModel);
        }

        private async Task<ViewResult> PagesView(string directory)
        {
            var isRootDirectory = string.IsNullOrWhiteSpace(directory);
            var pageDirectories = await _pageRepository.FindPublishedDirectoriesAsync(directory);
            var directories = pageDirectories.ToList();

            if (isRootDirectory)
            {
                directories.Add(new Directory(_contentSettings.BlogDirectoryName));
                directories = directories.OrderBy(d => d.Name).ToList();
            }

            var pages = await _pageRepository.FindPublishedPagesAsync(directory);
            var viewModel = new PagesViewModel(directory, directories, pages);

            return View("Pages", viewModel);
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