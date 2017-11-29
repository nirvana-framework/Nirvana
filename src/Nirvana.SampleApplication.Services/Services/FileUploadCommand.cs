using Nirvana.CQRS;

namespace Nirvana.SampleApplication.Services.Services
{
    public abstract class FileUploadCommand : Command<Nop>
    {
        public byte[] FileData { get; set; }
        public string FileMimeType { get; set; }
        public string FileName { get; set; }
    }
}