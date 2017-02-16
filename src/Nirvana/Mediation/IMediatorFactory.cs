using System;
using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.Util.Extensions;

namespace Nirvana.Mediation
{
    public enum MediatorStrategy
    {
        ForwardToWeb,
        HandleInProc,
        ForwardToQueue,
    }

    public interface IChildMediatorFactory: IMediatorFactory
    {

    }

    public interface IMediatorFactory
    {
        IMediator GetMediator(Type messageType);
        CommandResponse<TResult> Command<TResult>(Command<TResult> command);
        QueryResponse<TResult> Query<TResult>(Query<TResult> query);
        UIEventResponse Notification<TResult>(UiEvent<TResult> uiNotification);
        InternalEventResponse InternalEvent(InternalEvent internalEvent);
        bool ChildCommands { get; }
        
    }

    public class ChildMediatorFactory : MediatorFactoryBase, IChildMediatorFactory
    {
        public override  bool ChildCommands => true;
    }

    public class MediatorFactory : MediatorFactoryBase
    {
        public override bool ChildCommands => false;
    }



    public abstract class MediatorFactoryBase: IMediatorFactory
    {

        public abstract bool ChildCommands { get;}
        public IMediator GetMediator(Type messageType)
        {
            var mediatorStrategy = GetMediatorStrategy(messageType, ChildCommands);
            return GetMediatorByStrategy(mediatorStrategy);
        }

        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            return GetMediator(command.GetType()).Command(command);
        }

        public QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            return GetMediator(query.GetType()).Query(query);
        }

        public UIEventResponse Notification<TResult>(UiEvent<TResult> uiNotification)
        {
            return GetMediator(uiNotification.GetType()).UiNotification(uiNotification);
        }
        public InternalEventResponse InternalEvent(InternalEvent internalEvent)
        {
            return GetMediator(internalEvent.GetType()).InternalEvent(internalEvent);
        }


        private IMediator GetMediatorByStrategy(MediatorStrategy mediatorStrategy)
        {
            if (mediatorStrategy == MediatorStrategy.ForwardToWeb)
            {
                return GetWebMediator();
            }
            if (mediatorStrategy == MediatorStrategy.ForwardToQueue)
            {
                return GetQueueMediator();
            }
            return GetInProcMediator();
        }

        private MediatorStrategy GetMediatorStrategy(Type messageType, bool isChildTask = false)
        {
            if (messageType.IsQuery()
                || (messageType.IsUiNotification() && NirvanaSetup.IsInProcess(TaskType.UiNotification, isChildTask))
                || (messageType.IsCommand() && NirvanaSetup.IsInProcess(TaskType.Command, isChildTask))
                || (messageType.IsInternalEvent() && NirvanaSetup.IsInProcess(TaskType.InternalEvent, isChildTask))
                )
            {
                // Only commands can be offloaded currently
                return MediatorStrategy.HandleInProc;
            }

            if (
                 (messageType.IsUiNotification() && NirvanaSetup.ShouldForwardToWeb(TaskType.UiNotification, isChildTask))
                || (messageType.IsCommand() && NirvanaSetup.ShouldForwardToWeb(TaskType.Command, isChildTask))
                || (messageType.IsInternalEvent() && NirvanaSetup.ShouldForwardToWeb(TaskType.InternalEvent, isChildTask))
               )
            {
                return MediatorStrategy.ForwardToWeb;
            }

            if (
                (messageType.IsUiNotification() && NirvanaSetup.ShouldForwardToQueue(TaskType.UiNotification, isChildTask))
                || (messageType.IsCommand() && NirvanaSetup.ShouldForwardToQueue(TaskType.Command, isChildTask))
                || (messageType.IsInternalEvent() && NirvanaSetup.ShouldForwardToQueue(TaskType.InternalEvent, isChildTask))
                )
            {
                return MediatorStrategy.ForwardToQueue;
            }
            throw new NotImplementedException("Execution strategy could not be determined.  Please check your configuration.");


        }



        private IMediator GetWebMediator()
        {
            return (IWebMediator)NirvanaSetup.GetService(typeof(IWebMediator));
        }

        private IMediator GetQueueMediator()
        {
            return (IQueueMediator)NirvanaSetup.GetService(typeof(IQueueMediator));
        }
        private static IMediator GetInProcMediator()
        {
            return (IMediator)NirvanaSetup.GetService(typeof(IMediator));
        }

    }

   
}