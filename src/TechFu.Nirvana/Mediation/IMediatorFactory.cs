using System;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.Mediation
{
    public enum MediatorStrategy
    {
        ForwardToWeb,
        HandleInProc
    }

    public interface IMediatorFactory
    {
        IMediator GetMediator(Type messageType);
        CommandResponse<TResult> Command<TResult>(Command<TResult> command);
        QueryResponse<TResult> Query<TResult>(Query<TResult> query);
    }

    public class MediatorFactory : IMediatorFactory
    {
        public IMediator GetMediator(Type messageType)
        {
            var mediatorStrategy = GetMediatorStrategy(messageType);
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

        private IMediator GetMediatorByStrategy(MediatorStrategy mediatorStrategy)
        {
            if (mediatorStrategy == MediatorStrategy.ForwardToWeb)
            {
                return GetWebMediator();
            }
            return GetInProcMediator();
        }

        private MediatorStrategy GetMediatorStrategy(Type messageType)
        {
            if (messageType.IsQuery() 
                || messageType.IsUiNotification() 
                ||( messageType.IsCommand() && NirvanaSetup.MediationStrategy == MediationStrategy.InProcess))
            {
                // Only commands can be offloaded currently
                return MediatorStrategy.HandleInProc;
            }

            if (NirvanaSetup.MediationStrategy == MediationStrategy.ForwardToWeb)
            {
                return MediatorStrategy.ForwardToWeb;
            }
            throw new NotImplementedException("Currently all children must be handed in proc in synchronously.");


        }



        private IMediator GetWebMediator()
        {
            return (IWebMediator) NirvanaSetup.GetService(typeof(IWebMediator));
        }
  

        private static IMediator GetInProcMediator()
        {
            return (IMediator) NirvanaSetup.GetService(typeof(IMediator));
        }
    }
}