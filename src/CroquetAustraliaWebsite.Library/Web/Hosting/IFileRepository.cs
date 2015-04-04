using System;
using System.Web.Caching;

namespace CroquetAustraliaWebsite.Library.Web.Hosting
{
    public interface IFileRepository
    {
        bool FileExists(string path);
        CacheDependency GetCacheDependency(string virtualPath, DateTime utcStart);
        string GetFullPath(string path);
    }
}