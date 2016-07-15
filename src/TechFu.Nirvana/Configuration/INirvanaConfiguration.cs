using System.Configuration;

namespace TechFu.Nirvana.Configuration
{
    public interface INirvanaConfiguration
    {
        string CommandEndpoint { get; }
        string QueryEndpoint { get; }
        string NotificationEndpoint { get; }
    }

    public class NirvanaConfiguration : INirvanaConfiguration
    {
        public string CommandEndpoint => ConfigurationManager.AppSettings["NirvanaConfig.CommandEndpoint"] ??"";
        public string QueryEndpoint => ConfigurationManager.AppSettings["NirvanaConfig.QueryEndpoint"] ?? "";
        public string NotificationEndpoint => ConfigurationManager.AppSettings["NirvanaConfig.NotificationEndpoint"] ?? "";
    }
}