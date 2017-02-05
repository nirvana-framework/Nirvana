using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Data;
using TechFu.Nirvana.EventStoreSample.Domain.Domain.Security;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Command;
using TechFu.Nirvana.EventStoreSample.Services.Shared.Services.Security.Events;
using TechFu.Nirvana.Mediation;
using TechFu.Nirvana.Util.Tine;

namespace TechFu.Nirvana.EventStoreSample.Domain.Handlers.Security.Commands
{
    public class CreateAnonymousUserHandler : BaseNoOpCommandHandler<CreateAnonymousUserCommand>
    {
        private readonly IRepository<object> _repository;

        public CreateAnonymousUserHandler(IRepository<object> repository, IChildMediatorFactory mediator)
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