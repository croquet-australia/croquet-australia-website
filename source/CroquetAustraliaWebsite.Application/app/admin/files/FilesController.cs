using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Anotar.NLog;
using Casper.Core;
using Casper.Domain.Features.Files.Commands;
using Casper.Domain.Infrastructure.Messaging;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Authentication.Domain;
using CroquetAustraliaWebsite.Library.Web.Mvc;

namespace CroquetAustraliaWebsite.Application.App.admin.files
{
    [RoutePrefix("admin/files")]
    public class FilesController : AdminController
    {
        private readonly ICommandBus _commandBus;
        private readonly ISlugFactory _slugFactory;
        private readonly IUserRepository _userRepository;

        public FilesController(ICommandBus commandBus, ISlugFactory slugFactory, IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _slugFactory = slugFactory;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult> Upload(HttpPostedFileWrapper file, string directory = "")
        {
            LogTo.Trace("Upload(file: {0}, directory: {1})", file.FileName, directory);

            try
            {
                var urlFriendlyFileNameWithExtension = file.FileName.ToUrlFriendlyFileNameWithExtension(_slugFactory);
                var user = await this.GetDomainUserAsync(_userRepository);
                var author = user.ToAuthor();
                var published = user.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow);
                var command = new UploadFile(file, directory, urlFriendlyFileNameWithExtension, published, author);

                await _commandBus.SendCommandAsync(command);

                var savedAs = "/" + $"{directory}/{urlFriendlyFileNameWithExtension}".ToUnixSlashes().Trim('/');

                return new JsonResult {Data = new {savedAs}};
            }
            catch (Exception exception)
            {
                var message = $"Could not upload '{file.FileName}'.";
                LogTo.ErrorException(message, exception);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, message);
            }
        }
    }
}