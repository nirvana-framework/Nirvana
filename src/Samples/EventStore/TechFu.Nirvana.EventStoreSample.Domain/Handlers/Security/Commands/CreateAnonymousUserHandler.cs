using Nirvana.CQRS;
using Nirvana.Data;
using Nirvana.Mediation;
using Nirvana.Util.Tine;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.Security;
using TechFu.Nirvana.EventStoreSample.Services.Shared;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Command;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Events;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.Security.Commands
{
    public class CreateAnonymousUserHandler : BaseNoOpCommandHandler<CreateAnonymousUserCommand>
    {
        private readonly IRepository<SecurityRoot> _repository;

        public CreateAnonymousUserHandler(IRepository<SecurityRoot> repository, IChildMediatorFactory mediator)
            : base(mediator)
        {
            _repository = repository;
        }

        public override CommandResponse<Nop> Handle(CreateAnonymousUserCommand task)
        {
            var user = _repository.Get<SiteUser>(task.SessionId);
            if (user != null)
                return CommandResponse.Succeeded(Nop.NoValue, "User already existed");
            user = new SiteUser
            {
                Id = task.SessionId,
                IsAnonomous = true,
                LastLogin = new SystemTime().UtcNow(),
                LoginCount = 1,
                Name = ""
            };
            _repository.SaveOrUpdate(user);
            Mediator.InternalEvent(new UserCreatedEvent
            {
                UserId = user.Id
            });
            return CommandResponse.Succeeded(Nop.NoValue);
        }
    }
}