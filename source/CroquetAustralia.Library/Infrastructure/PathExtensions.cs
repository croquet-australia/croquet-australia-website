using System;
using System.IO;

namespace CroquetAustralia.Library.Infrastructure
{
    public static class PathExtensions
    {
        public static bool IsMarkdownFile(string path)
        {
            var extension = Path.GetExtension(path) ?? "";
            var result = extension.Equals(".md", StringComparison.OrdinalIgnoreCase);

            return result;
        }

        public static string GetRelativePath(string parentDirectory, string fullFilePath)
        {
            var parentDirectoryLength = parentDirectory.Length;

            if (!parentDirectory.EndsWith("\\"))
            {
                parentDirectoryLength += 1;
            }

            return fullFilePath.Substring(parentDirectoryLength);
        }

        public static string GetDirectory(string fullFilePath)
        {
            var file = new FileInfo(fullFilePath);
            var directory = file.Directory;

            if (directory != null)
            {
                return directory.FullName;
            }

            var exception = new ArgumentException("Value does not contain a directory.", nameof(fullFilePath));

            exception.Data.Add("fullFilePath", fullFilePath);

            throw exception;
        }
    }
}