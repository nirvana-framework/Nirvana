using System.Diagnostics;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.Mediation
{
    
    public interface IMediator
    {
        CommandResponse<TResult> Command<TResult>(Command<TResult> command);
        QueryResponse<TResult> Query<TResult>(Query<TResult> query);
    }

    public interface IWebMediator : IMediator{}
    public interface ILocalMediator : IMediator { }
    public interface IQueueMediator : IMediator { }
    public interface IEventStoreMediator : IMediator { }

}