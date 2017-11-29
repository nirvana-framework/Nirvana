using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.CQRS.Queue;
using Nirvana.Logging;
using Nirvana.Util.Compression;

namespace Nirvana.AzureQueues.Handlers
{
    public class AzureQueueMessage : QueueMessage
    {
        private readonly ICompression _compression;
        private readonly ILogger _logger;

        

        public AzureQueueMessage(CloudQueueMessage message, NirvanaTaskInformation messageTypeRouting, ILogger logger,ICompression compression) : base(messageTypeRouting)
        {
            _logger = logger;
            _compression = compression;
            Id = message.Id;
            DequeueCount = message.DequeueCount;
            NextVisibleTime = message.NextVisibleTime?.UtcDateTime;
            InsertionTime = message.InsertionTime?.UtcDateTime;
            ExpirationTime = message.ExpirationTime?.UtcDateTime;
            SetText(GetText(message));
        }

        private string GetText(CloudQueueMessage message)
        {
            try
            {
                var bytes = message.AsBytes;

                return _compression.IsCompressed(bytes)
                    ? Encoding.UTF8.GetString(_compression.Decompress(bytes))
                    : message.AsString;
            }
            catch
            {
                return message.AsString;
            }
        }
    }
}