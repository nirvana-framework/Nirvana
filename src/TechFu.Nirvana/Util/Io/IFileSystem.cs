using System.IO;

namespace TechFu.Nirvana.Util.Io
{
    public interface IFileSystem
    {
        Stream CreateStream(string filename);
        Stream ReadStream(string filename);
        Stream WriteStream(string filename);
        string GetTempFileName();
        bool Exists(string filename);
        void Delete(string filename);
        string GetFullPath(string path);
    }
}
