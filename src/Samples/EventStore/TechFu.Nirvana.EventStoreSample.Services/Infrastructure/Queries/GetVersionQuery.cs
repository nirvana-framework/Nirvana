using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Infrastructure.Queries
{
    [InfrastructureRoot("getversion")]
    public class GetVersionQuery:Query<VersionModel>
    {

    }

    public class VersionModel
    {
        public string Version { get; set; }
    }
}