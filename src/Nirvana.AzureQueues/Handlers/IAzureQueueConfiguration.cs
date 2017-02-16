using System.Configuration;

namespace Nirvana.AzureQueues.Handlers
{
    public interface IAzureQueueConfiguration
    {
        string ConnectionString { get; }
    }

    public class AzureQueueConfiguration : IAzureQueueConfiguration
    {
        public string ConnectionString => ConfigurationManager.AppSettings["Nirvana.AzureQueue.ConnectionString"];
    }


}