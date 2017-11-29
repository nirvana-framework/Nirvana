using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.AzureQueues.Handlers;
using Nirvana.Configuration;
using Nirvana.CQRS.Queue;
using Nirvana.JsonSerializer;
using Nirvana.Logging;
using Nirvana.Mediation;
using Nirvana.Mediation.Implementation;
using Nirvana.Util.Io;
using Nirvana.WebUtils;
using StructureMap;

namespace Nirvana.Sample.Infrastructure.IoC
{
    public class TestRegistry:Registry
    {
        public TestRegistry()
        {

        }
    }


    public class NirvanaRegistry : Registry
    {
        public NirvanaRegistry()
        {
            //Nirvana Settings
            For<IMediator>().Use<Mediator>();
            For<IWebMediator>().Use<WebMediator>();
            For<IQueueFactory>().Use<AzureQueueFactory>();
            For<INirvanaConfiguration>().Use<NirvanaConfiguration>();
            For<ISerializer>().Singleton().Use<Serializer>();
            For<IQueueController>().Singleton().Use<AzureQueueController>();
            For<IMediatorFactory>().Singleton().Use<MediatorFactory>();
            For<IChildMediatorFactory>().Singleton().Use<ChildMediatorFactory>();
            For<ILogger>().Singleton().Use(x=>new ConsoleLogger(true,true,true,true,true,false));
        }
    }

}
