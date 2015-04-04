using System.IO;
using System.Threading.Tasks;
using Anotar.LibLog;
using CroquetAustraliaWebsite.Library.Settings;
using Microsoft.VisualBasic.Devices;

namespace CroquetAustraliaWebsite.Library.Content
{
    public class PublishedContentRepository : IPublishedContentRepository
    {
        private readonly PublishedContentRepositorySettings _publishedRepositorySettings;
        private readonly GitContentRepositorySettings _gitRepositorySettings;

        public PublishedContentRepository(PublishedContentRepositorySettings publishedRepositorySettings, GitContentRepositorySettings gitRepositorySettings)
        {
            _publishedRepositorySettings = publishedRepositorySettings;
            _gitRepositorySettings = gitRepositorySettings;
        }

        public void Start()
        {
            if (Directory.Exists(_publishedRepositorySettings.Directory))
            {
                return;
            }
            PublishContent().Wait();
        }

        public void Publish(string path)
        {
            LogTo.Trace("Publish(path: {0})", path);

            var fullGitPath = Path.Combine(_gitRepositorySettings.Directory, path);
            var fullPublishedPath = Path.Combine(_publishedRepositorySettings.Directory, path);

            if (path.EndsWith(".md"))
            {
                PublishMarkdownFile(fullGitPath, fullPublishedPath.Replace(".md", ".cshtml"));
            }
            else
            {
                File.Copy(fullGitPath, fullPublishedPath);
            }

            LogTo.Info("Published {0}.", path);
        }

        private void PublishMarkdownFile(string fullGitPath, string fullHtmlPath)
        {
            var markdown = new MarkdownSharp.Markdown();
            var html = markdown.Transform(File.ReadAllText(fullGitPath));

            File.WriteAllText(fullHtmlPath, html);
        }

        private async Task PublishContent()
        {
            await CopyFilesGitRepository();
            await DeleteFilesNotInGitRepository();
        }

        private Task CopyFilesGitRepository()
        {
            return Task.Run(() => CopyFilesGitRepositorySync());
        }

        private void CopyFilesGitRepositorySync()
        {
            // todo: make async
            var computer = new Computer();
            computer.FileSystem.CopyDirectory(_gitRepositorySettings.Directory, _publishedRepositorySettings.Directory, true);
        }

        private static Task DeleteFilesNotInGitRepository()
        {
            // todo
            return Task.FromResult(0);
        }
    }
}