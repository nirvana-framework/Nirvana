using System;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.ViewModels;

namespace TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Query
{
    [SecurityRoot("GetNewSessionViewModelQuery")]
    public class GetNewSessionViewModelQuery:Query<SessionViewModel>
    {
        public Guid SessionId { get; set; }
    }
}
