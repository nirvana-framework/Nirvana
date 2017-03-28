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

        public MediatorStrategy GetMediatorStrategy(Type messageType, bool isChildTask = false)
        {
            if (ShouldHandleInProc(messageType, isChildTask))
            {
                return MediatorStrategy.HandleInProc;
            }

            if (ShouldForwardToWeb(messageType, isChildTask))
            {
                return MediatorStrategy.ForwardToWeb;
            }

            if (ShouldForwardToQueue(messageType, isChildTask))
            {
                return MediatorStrategy.ForwardToQueue;
            }

            throw new NotImplementedException(
                "Execution strategy could not be determined.  Please check your configuration.");
        }

        private bool ShouldHandleInProc(Type messageType, bool isChildTask)
        {
            var taskInfo = _setup.FindTypeDefinition(messageType);
            if (taskInfo == null)
            {
                return false;
            }
            var action = isChildTask ? taskInfo.ChildAction : taskInfo.TopLevelAction;
            return action==MediationStrategy.InProcess;
        }

        private bool ShouldForwardToWeb(Type messageType, bool isChildTask)
        {
            var taskInfo = _setup.FindTypeDefinition(messageType);
            if (taskInfo == null)
            {
                return false;
            }
            var action = isChildTask ? taskInfo.ChildAction : taskInfo.TopLevelAction;
            return action == MediationStrategy.ForwardToWeb;
        }

        private bool ShouldForwardToQueue(Type messageType, bool isChildTask)
        {
            var taskInfo = _setup.FindTypeDefinition(messageType);
            if (taskInfo == null)
            {
                return false;
            }
            var action = isChildTask ? taskInfo.ChildAction : taskInfo.TopLevelAction;
            return action == MediationStrategy.ForwardToQueue;
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