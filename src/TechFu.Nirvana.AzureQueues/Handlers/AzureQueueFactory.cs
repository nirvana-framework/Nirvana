using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using TechFu.Nirvana.CQRS.Queue;
using TechFu.Nirvana.Util.Compression;
using TechFu.Nirvana.Util.Io;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.AzureQueues.Handlers
{
    public class AzureQueueFactory : IQueueFactory
    {
        private readonly Lazy<CloudQueueClient> _client;
        private readonly ICompression _compression;
        private readonly ISerializer _serializer;
        private readonly ISystemTime _systemTime;

        protected Func<Type, string> GetQueueName { get; set; }

        public AzureQueueFactory(IAzureQueueConfiguration configuration,ISerializer serializer, ISystemTime systemTime, ICompression compression)
        {
            _serializer = serializer;
            _systemTime = systemTime;
            _compression = compression;
            _client = new Lazy<CloudQueueClient>(() =>
            {
                var account = CloudStorageAccount.Parse(configuration.ConnectionString);
                return account.CreateCloudQueueClient();
            });
           
        }

        public virtual IQueue GetQueue(Type messageType)
        {
            return new AzureStorageQueue(_client.Value,"test" ,messageType)
                .SetTime(_systemTime)
                .SetSerializer(_serializer)
                .SetCompression(_compression);
        }
    }
}