namespace Nirvana.CQRS
{
    public class ValidationMessage
    {
        public ValidationMessage(MessageType mesageType,string key, string message)
        {
            MessageType = mesageType;
            Key = key;
            Message = message;
        }

        public MessageType MessageType { get; set; }
        public string Key { get; set; }
        public string Message { get; set; }
    }

    internal enum ValidationMessageType
    {
        Warning,
        Error,
        Exception,
        Info
    }
}