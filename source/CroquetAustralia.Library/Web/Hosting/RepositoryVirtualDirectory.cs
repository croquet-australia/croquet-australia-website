using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Anotar.LibLog;

namespace CroquetAustralia.Library.Web.Hosting
{
    public class RepositoryVirtualDirectory : VirtualDirectory
    {
        private readonly string _fullPath;

        public RepositoryVirtualDirectory(string virtualPath, string fullPath)
            : base(virtualPath)
        {
            _fullPath = fullPath;
        }

        public override IEnumerable Directories
        {
            get
            {
                LogTo.Warn("todo: Directories");
                throw new NotImplementedException("todo: Directories");
            }
        }

        public override IEnumerable Files
        {
            get { return Directory.EnumerateFiles(_fullPath).Select(fullPath => new RepositoryVirtualFile(GetFileVirtualPath(fullPath), fullPath)); }
        }

        public override IEnumerable Children
        {
            get
            {
                LogTo.Warn("todo: Children");
                throw new NotImplementedException("todo: Children");
            }
        }

        private string GetFileVirtualPath(string fullPath)
        {
            var fileName = Path.GetFileName(fullPath);

            if (fileName == null)
            {
                LogTo.Warn("Expected GetFileVirtualPath(fullPath: {0}) to include a file name.", fullPath);
            }

            var virtualPath = Path.Combine(VirtualPath, fileName ?? "");

            LogTo.Debug("GetFileVirtualPath(fullPath: {0}) => {1}", fullPath, virtualPath);

            return virtualPath;
        }
    }
}