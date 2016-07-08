using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Queries
{
    [AggregateRoot(RootType.Infrastructure)]
    public class GetVersionQuery:Query<VersionModel>
    {

    }

    public class VersionModel
    {
        public string Version { get; set; }
    }
}