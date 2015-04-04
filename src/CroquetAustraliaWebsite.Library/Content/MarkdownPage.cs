using System.IO;
using Anotar.LibLog;

namespace CroquetAustraliaWebsite.Library.Content
{
    public class MarkdownPage
    {
        public static void PublishPage(string fullDirectoryPath, string pageName, string content, IGitContentRepository gitRepository, IPublishedContentRepository publishedRepository)
        {
            LogTo.Trace("PublishPage(fullDirectoryPath: {0}, pageName: {1}, content: {2}, gitRepository: {3})", fullDirectoryPath, pageName, content, gitRepository);

            var fullPath = SavePage(fullDirectoryPath, pageName, content);
            var gitPath = fullPath.Substring(gitRepository.Directory.Length).TrimStart('\\');

            gitRepository.CommitAndPush(gitPath);
            publishedRepository.Publish(gitPath);

            LogTo.Info("Published {0}.", gitPath);
        }

        private static string SavePage(string fullDirectoryPath, string pageName, string content)
        {
            var fileName = GetFileName(pageName);
            var fullPath = Path.Combine(fullDirectoryPath, fileName);

            File.WriteAllText(fullPath, content);
            LogTo.Info("Saved {0}.", fullPath);

            return fullPath;
        }

        private static string GetFileName(string pageName)
        {
            return pageName + ".md";
        }
    }
}