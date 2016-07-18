using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.Mediation
{
    public interface IMediator
    {
        CommandResponse<TResult> Command<TResult>(Command<TResult> command);
        QueryResponse<TResult> Query<TResult>(Query<TResult> query);
        UIEventResponse UiNotification<T>(UiEvent<T> uiEevent);
        InternalEventResponse InternalEvent<T>(InternalEvent<T> internalEvent);
    }

    public interface IWebMediator : IMediator{}
    public interface ILocalMediator : IMediator{}
    public interface IQueueMediator : IMediator{}
    public interface IEventStoreMediator : IMediator{}

}