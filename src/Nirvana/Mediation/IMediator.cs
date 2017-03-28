using Nirvana.CQRS;

namespace Nirvana.Mediation
{
    public interface IMediator
    {
        CommandResponse<TResult> Command<TResult>(Command<TResult> command);
        QueryResponse<TResult> Query<TResult>(Query<TResult> query);
        UIEventResponse UiNotification<T>(UiNotification<T> uiEevent);
        InternalEventResponse InternalEvent(InternalEvent internalEvent);
    }

    public interface IWebMediator : IMediator{}
    public interface ILocalMediator : IMediator{}
    public interface IQueueMediator : IMediator{}
    public interface IEventStoreMediator : IMediator{}

}