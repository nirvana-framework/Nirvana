using System.IO;

namespace TechFu.Nirvana.CQRS
{
    public class FileQueryResult
    {
        public bool IsStream { get; set; }
        public byte[] FileBytes { get; set; }
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}