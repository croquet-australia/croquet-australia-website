using System;
using System.Web.Caching;

namespace CroquetAustralia.Library.Web.Hosting
{
    public interface IFileRepository
    {
        bool DirectoryExists(string path);
        bool FileExists(string path);
        CacheDependency GetCacheDependency(string virtualPath, DateTime utcStart);
        string GetFullPath(string path);
    }
}