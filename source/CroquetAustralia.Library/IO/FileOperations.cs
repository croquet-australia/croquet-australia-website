using System.IO;

namespace CroquetAustralia.Library.IO
{
    public class FileOperations : IFileOperations
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
