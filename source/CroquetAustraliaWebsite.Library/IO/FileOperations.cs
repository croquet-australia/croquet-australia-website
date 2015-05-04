using System.IO;

namespace CroquetAustraliaWebsite.Library.IO
{
    public class FileOperations : IFileOperations
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
