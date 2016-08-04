using System;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Handlers.ProductCatalog.Common;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Security.Command;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Security.Query;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Security.ViewModels;
using TechFu.Nirvana.Mediation;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.Security.Queries
{
    public class GetSessionViewModelHandler : ViewModelQueryBase<GetNewSessionViewModelQuery, SessionViewModel>
    {
        public GetSessionViewModelHandler(IViewModelRepository repository, IMediatorFactory mediator)
            : base(repository, mediator)
        {
        }

        public override QueryResponse<SessionViewModel> Handle(GetNewSessionViewModelQuery query)
        {
            //First Time User
            if (query.SessionId == Guid.Empty)
            {
                var sessionId = Guid.NewGuid();
                Mediator.Command(new CreateNewSessionViewModelCommand {SessionId = sessionId});

                return QueryResponse.Succeeded(new SessionViewModel
                {
                    Id = sessionId,
                    RootEntityKey = sessionId,
                    Name = ""
                });
            }


            //return user w/ cookie
            return GetFromViewModelRepository(query, q => query.SessionId);
        }
    }
}