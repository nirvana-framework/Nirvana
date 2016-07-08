namespace TechFu.Nirvana.EventStoreSample.Domain.Infrastructure
{
    public interface IApplicationConfiguration
    {
        string Environment { get; }
        string SiteUrl { get; }
        string DataStoreConnectionString { get; }
        string DataStoreConnectionStringDr { get; }
        IAuthenticationConfiguration AuthenticationConfiguration { get; }
    }

    public interface IAuthenticationConfiguration
    {
        bool Enabled { get; }
    }
}