using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Anotar.NLog;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.Infrastructure;

namespace CroquetAustraliaWebsite.Application.App.admin.home
{
    [RoutePrefix("admin")]
    public class HomeController : AdminController
    {
        private readonly IGitContentRepository _gitRepository;
        private readonly IPublishedContentRepository _publishedRepository;

        public HomeController(IGitContentRepository gitRepository, IPublishedContentRepository publishedRepository)
        {
            _gitRepository = gitRepository;
            _publishedRepository = publishedRepository;
        }

        [Route("{directory?}")]
        public async Task<ViewResult> Index(string directory = @"")
        {
            return View(await IndexViewModel.CreateInstance(_gitRepository.Directory, directory));
        }

        [Route("new-page/{directory?}")]
        public ViewResult NewPage(string directory = "")
        {
            LogTo.Trace("NewPage(directory: {0})", directory ?? "<null>");

            Argument.CannotBeNull("directory", directory);

            return View(new NewPageViewModel(_gitRepository.Directory, directory));
        }

        [HttpPost]
        [Route("new-page")]
        [ValidateAntiForgeryToken]
        public ActionResult NewPage(NewPageViewModel viewModel)
        {
            LogTo.Trace("POST: NewPage(viewModel: {0})", viewModel);

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

                // todo: delete // return Redirect(string."~/admin/content?directory={0}")
                return RedirectToAction("index", new {directory = viewModel.Directory});
            }

            LogTo.Debug("Model is invalid.{0}{1}", Environment.NewLine, ModelState.ErrorsAsLoggingString());
            return View(viewModel);
        }

        private ViewResult PublishNewPage(NewPageViewModel viewModel)
        {

            throw new NotImplementedException("todo");
        }
    }
}