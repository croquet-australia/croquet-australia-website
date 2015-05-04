using System.IO;
using System.Web.Hosting;

namespace CroquetAustraliaWebsite.Library.Web.Hosting
{
    public class RepositoryVirtualFile : VirtualFile
    {
        private readonly string _fullPath;

        public RepositoryVirtualFile(string virtualPath, string fullPath) : base(virtualPath)
        {
            _fullPath = fullPath;
        }

        public override Stream Open()
        {
            return File.Open(_fullPath, FileMode.Open, FileAccess.Read);
        }
    }
}