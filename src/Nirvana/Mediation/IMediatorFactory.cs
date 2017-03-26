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
        ForwardToQueue
    }

    public interface IChildMediatorFactory : IMediatorFactory
    {
    }

    public interface IMediatorFactory
    {
        bool ChildCommands { get; }
        IMediator GetMediator(Type messageType);
        CommandResponse<TResult> Command<TResult>(Command<TResult> command);
        QueryResponse<TResult> Query<TResult>(Query<TResult> query);
        UIEventResponse Notification<TResult>(UiEvent<TResult> uiNotification);
        InternalEventResponse InternalEvent(InternalEvent internalEvent);
    }

    public class ChildMediatorFactory : MediatorFactoryBase, IChildMediatorFactory
    {
        public ChildMediatorFactory(NirvanaSetup setup) : base(setup)
        {
        }

        public override bool ChildCommands => true;
    }

    public class MediatorFactory : MediatorFactoryBase
    {
        public override bool ChildCommands => false;

        public MediatorFactory(NirvanaSetup setup) : base(setup)
        {
        }
    }


    public abstract class MediatorFactoryBase : IMediatorFactory
    {
        private readonly NirvanaSetup _setup;

        protected MediatorFactoryBase(NirvanaSetup setup)
        {
            _setup = setup;
        }

        public abstract bool ChildCommands { get; }

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
                || messageType.IsUiNotification() && _setup.IsInProcess(TaskType.UiNotification, isChildTask)
                || messageType.IsCommand() && _setup.IsInProcess(TaskType.Command, isChildTask)
                || messageType.IsInternalEvent() && _setup.IsInProcess(TaskType.InternalEvent, isChildTask)
            )
            {
                return MediatorStrategy.HandleInProc;
            }

            if (
                messageType.IsUiNotification() && _setup.ShouldForwardToWeb(TaskType.UiNotification, isChildTask)
                || messageType.IsCommand() && _setup.ShouldForwardToWeb(TaskType.Command, isChildTask)
                || messageType.IsInternalEvent() && _setup.ShouldForwardToWeb(TaskType.InternalEvent, isChildTask)
            )
            {
                return MediatorStrategy.ForwardToWeb;
            }

            if (
                messageType.IsUiNotification() &&
                _setup.ShouldForwardToQueue(TaskType.UiNotification, isChildTask, messageType)
                || messageType.IsCommand() && _setup.ShouldForwardToQueue(TaskType.Command, isChildTask, messageType)
                ||
                messageType.IsInternalEvent() &&
                _setup.ShouldForwardToQueue(TaskType.InternalEvent, isChildTask, messageType)
            )
            {
                return MediatorStrategy.ForwardToQueue;
            }
            throw new NotImplementedException(
                "Execution strategy could not be determined.  Please check your configuration.");
        }


        private IMediator GetWebMediator()
        {
            return (IWebMediator) _setup.GetService(typeof(IWebMediator));
        }

        private IMediator GetQueueMediator()
        {
            return (IQueueMediator) _setup.GetService(typeof(IQueueMediator));
        }

        private IMediator GetInProcMediator()
        {
            return (IMediator) _setup.GetService(typeof(IMediator));
        }
    }
}