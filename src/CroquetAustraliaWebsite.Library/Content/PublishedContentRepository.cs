using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using CroquetAustraliaWebsite.Library.Infrastructure;
using CroquetAustraliaWebsite.Library.Settings;
using OpenMagic.Extensions.Collections.Generic;

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
            PublishContentAsync().Wait();
        }

        public void Publish(string relativePath)
        {
            LogTo.Trace("Publish(relativePath: {0})", relativePath);

            var fullGitPath = Path.Combine(_gitRepositorySettings.Directory, relativePath);
            var fullPublishedPath = Path.Combine(_publishedRepositorySettings.Directory, relativePath);
            var publishedDirectory = PathExtensions.GetDirectory(fullPublishedPath);

            Directory.CreateDirectory(publishedDirectory);
            File.Copy(fullGitPath, fullPublishedPath);

            LogTo.Info("Published {0}.", relativePath);
        }

        private async Task PublishContentAsync()
        {
            await CopyFilesInGitRepositoryAsync();
            await DeleteFilesNotInGitRepository();
        }

        private Task CopyFilesInGitRepositoryAsync()
        {
            return Task.Run(() => CopyFilesInGitRepository());
        }

        private void CopyFilesInGitRepository()
        {
            // todo: make async
            CopyFilesInGitRepository(_gitRepositorySettings.Directory);
        }

        private void CopyFilesInGitRepository(string sourceDirectory)
        {
            LogTo.Trace("CopyFilesInGitRepository(sourceDirectory: {0})", sourceDirectory);

            Directory.GetFiles(sourceDirectory)
                .ForEach(sourceFile => { Publish(PathExtensions.GetRelativePath(_gitRepositorySettings.Directory, sourceFile)); });

            Directory.GetDirectories(sourceDirectory)
                .Where(directory => !IsGitDirectory(directory))
                .ForEach(CopyFilesInGitRepository);
        }

        private static bool IsGitDirectory(string directory)
        {
            return directory.EndsWith(".git", StringComparison.OrdinalIgnoreCase);
        }

        private static Task DeleteFilesNotInGitRepository()
        {
            // todo
            return Task.FromResult(0);
        }
    }
}