using System;
using System.Collections;
using System.Linq;
using System.Web.Caching;
using System.Web.Hosting;
using Anotar.LibLog;
using NullGuard;

namespace CroquetAustraliaWebsite.Library.Web.Hosting
{
    public class RepositoryPathProvider : VirtualPathProvider
    {
        private readonly IFileRepository _repository;

        public RepositoryPathProvider(IFileRepository repository)
        {
            _repository = repository;
        }

        public override bool DirectoryExists(string virtualDir)
        {
            LogTo.Trace("DirectoryExists(virtualDir: {0})", virtualDir);
            return _repository.DirectoryExists(FormatVirtualPathForRepository(virtualDir)) || Previous.DirectoryExists(virtualDir);
        }

        public override bool FileExists(string virtualPath)
        {
            LogTo.Trace("FileExists(virtualPath: {0})", virtualPath);
            return _repository.FileExists(FormatVirtualPathForRepository(virtualPath)) || Previous.FileExists(virtualPath);
        }

        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            LogTo.Trace("GetDirectory(virtualDir: {0})", virtualDir);

            // Surprisingly we have to run Exists again to check it this virtualDirProvider want to get virtualDir.
            return _repository.DirectoryExists(FormatVirtualPathForRepository(virtualDir))
                ? new RepositoryVirtualDirectory(virtualDir, _repository.GetFullPath(virtualDir))
                : Previous.GetDirectory(virtualDir);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            LogTo.Trace("GetFile(virtualPath: {0})", virtualPath);

            // Surprisingly we have to run Exists again to check it this VirtualPathProvider want to get virtualPath.
            return _repository.FileExists(FormatVirtualPathForRepository(virtualPath))
                ? new RepositoryVirtualFile(virtualPath, _repository.GetFullPath(virtualPath)) 
                : Previous.GetFile(virtualPath);
        }

        private static string FormatVirtualPathForRepository(string virtualPath)
        {
            return virtualPath.TrimStart('~');
        }

        [return: AllowNull]
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return GetCacheDependencyImpl(virtualPath, virtualPathDependencies.Cast<string>().ToArray(), utcStart);
        }

        private CacheDependency GetCacheDependencyImpl(string virtualPath, string[] virtualPathDependencies, DateTime utcStart)
        {
            LogTo.Trace("GetCacheDependency(virtualPath: {0}, virtualPathDependencies: [{1}], DateTime: {2})", virtualPath, string.Join(", ", virtualPathDependencies), utcStart);

            if (!_repository.FileExists(FormatVirtualPathForRepository(virtualPath)))
            {
                return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }

            if (virtualPathDependencies.Length != 1)
            {
                LogTo.Warn("Expected virtualPathDependencies.Length to be 1 but it is {0}.", virtualPathDependencies.Length);
                return null;
            }

            if (virtualPath == virtualPathDependencies[0])
            {
                return _repository.GetCacheDependency(virtualPath, utcStart);
            }

            LogTo.Warn("Expected virtualPath '{0}' to equal virtualPathDependencies[0] '{1}'.", virtualPath, virtualPathDependencies[0]);
            return null;
        }
    }
}