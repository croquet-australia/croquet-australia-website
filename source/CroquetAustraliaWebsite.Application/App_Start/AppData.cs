using Anotar.NLog;
using CroquetAustraliaWebsite.Library.Content;

namespace CroquetAustraliaWebsite.Application
{
    public class AppData
    {
        private readonly IGitContentRepository _gitContentRepository;
        private readonly IPublishedContentRepository _publishedContentRepository;

        public AppData(IGitContentRepository gitContentRepository, IPublishedContentRepository publishedContentRepository)
        {
            _gitContentRepository = gitContentRepository;
            _publishedContentRepository = publishedContentRepository;
        }

        /// <summary>
        ///     Ensures the website's App_Data folder is installed and ready.
        /// </summary>
        public void Start()
        {
            LogTo.Trace("Start");

            _gitContentRepository.Start();
            _publishedContentRepository.Start();
        }
    }
}