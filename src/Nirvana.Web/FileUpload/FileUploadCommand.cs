using Nirvana.CQRS;

namespace Nirvana.Web.FileUpload
{
    public abstract class FileUploadCommand : Command<Nop>
    {
        public byte[] FileData { get; set; }
        public string FileMimeType { get; set; }
        public string FileName { get; set; }
    }
}