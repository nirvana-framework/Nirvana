using System;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Security.ViewModels;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Security.Query
{
    [SecurityRoot("GetNewSessionViewModelQuery")]
    public class GetNewSessionViewModelQuery:Query<SessionViewModel>
    {
        public Guid SessionId { get; set; }
    }
}
