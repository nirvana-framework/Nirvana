using System.IO;

namespace TechFu.Nirvana.Util.Io
{
    public class FileSystem : IFileSystem
    {
        public Stream CreateStream(string filename)
        {
            return File.Open(filename, FileMode.Create);
        }

        public Stream ReadStream(string filename)
        {
            return File.OpenRead(filename);
        }

        public Stream WriteStream(string filename)
        {
            return File.OpenWrite(filename);
        }

        public void Delete(string filename)
        {
            File.Delete(filename);
        }

        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public bool Exists(string filename)
        {
            return File.Exists(filename);
        }

        public string GetTempFileName()
        {
            return Path.GetTempFileName();
        }
    }
}