using System;
using System.IO;
using System.Web.Caching;
using Anotar.LibLog;

namespace CroquetAustralia.Library.Web.Hosting
{
    public class FileRepository : IFileRepository
    {
        private readonly string _rootDirectory;

        public FileRepository(string rootDirectory)
        {
            LogTo.Trace("ctor(rootDirectory: {0})", rootDirectory);

            if (!Directory.Exists(rootDirectory))
            {
                throw new ArgumentException($"rootDirectory {rootDirectory} does not exist.", nameof(rootDirectory));
            }
            _rootDirectory = rootDirectory;
        }

        public bool DirectoryExists(string path)
        {
            var fullPath = GetFullPath(path);
            var exists = Directory.Exists(fullPath);

            LogTo.Debug("DirectoryExists({0}) => {1}. fullPath: {2}.", path, exists, fullPath);

            return exists;
        }

        public bool FileExists(string path)
        {
            var fullPath = GetFullPath(path);
            var exists = File.Exists(fullPath);

            LogTo.Debug("FileExists({0}) => {1}. fullPath: {2}.", path, exists, fullPath);

            return exists;
        }

        public CacheDependency GetCacheDependency(string virtualPath, DateTime utcStart)
        {
            return new CacheDependency(GetFullPath(virtualPath), utcStart);
        }

        public string GetFullPath(string path)
        {
            // Path.Combine return paths if path starts with /. Hence the trimming :-)
            return Path.Combine(_rootDirectory, path.TrimStart('/'));
        }
    }
}