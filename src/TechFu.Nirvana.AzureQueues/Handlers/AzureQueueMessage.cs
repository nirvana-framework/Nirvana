using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.Util.Compression;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureQueueMessage : QueueMessage
    {
        private readonly ICompression _compression;

        public AzureQueueMessage(ICompression compression, CloudQueueMessage message, NirvanaTypeRoutingDefinition messageTypeRouting)
            : this(message, messageTypeRouting)
        {
            _compression = compression;
        }

        public AzureQueueMessage(CloudQueueMessage message, NirvanaTypeRoutingDefinition messageTypeRouting) : base(messageTypeRouting)
        {
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