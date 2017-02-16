using System.Configuration;

namespace Nirvana.Configuration
{
    public interface INirvanaConfiguration
    {
        string CommandEndpoint { get; }
        string QueryEndpoint { get; }
        string NotificationEndpoint { get; }
        string InternalEventEndpoint { get; }
    }

    public class NirvanaConfiguration : INirvanaConfiguration
    {
        public string CommandEndpoint => ConfigurationManager.AppSettings["NirvanaConfig.CommandEndpoint"] ??"";
        public string QueryEndpoint => ConfigurationManager.AppSettings["NirvanaConfig.QueryEndpoint"] ?? "";
        public string NotificationEndpoint => ConfigurationManager.AppSettings["NirvanaConfig.NotificationEndpoint"] ?? "";
        public string InternalEventEndpoint => ConfigurationManager.AppSettings["NirvanaConfig.InternalEventEndpoint"] ?? "";

    }
}