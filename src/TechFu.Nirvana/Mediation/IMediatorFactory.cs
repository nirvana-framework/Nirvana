using System;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.Mediation
{
    public enum MediatorStrategy
    {
        ForwardToWeb,
        HandleInProc
    }

    internal interface IMediatorFactory
    {
        IMediator GetMediator(Type messageType);
    }

    public class MediatorFactory : IMediatorFactory
    {
        public IMediator GetMediator(Type messageType)
        {
            var mediatorStrategy = GetMediatorStrategy(messageType);
            return GetMediatorByStrategy(mediatorStrategy);


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
                ||( messageType.IsCommand() && NirvanaSetup.WebMediationStrategy == WebMediationStrategy.None))
            {
                // Only commands can be offloaded currently
                return MediatorStrategy.HandleInProc;
            }

            if (NirvanaSetup.WebMediationStrategy == WebMediationStrategy.ForwardAll)
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