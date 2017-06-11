using System;
using System.IO;
using System.Linq;
using Nirvana.AzureQueues.Handlers;
using Nirvana.Configuration;
using Nirvana.CQRS.Queue;
using Nirvana.Domain;
using Nirvana.JsonSerializer;
using Nirvana.Logging;
using Nirvana.Mediation;
using Nirvana.Util.Compression;
using Nirvana.Util.Io;
using Nirvana.Util.Tine;
using Should;
using Xunit;
using Xunit.Abstractions;

namespace Nirvana.AzureQueues.IntegrationTests.AzureQueue
{
    public class QueueControllerTests
    {
        private readonly ITestOutputHelper _output;

        public Func<string, object, bool> AttributeMatchingFunctionStub
            => (x, y) => x == ((AggregateRootAttribute) y).RootName;

        private AzureQueueController Sut;
        private NirvanaSetup Setup;


        protected int MessageCount;
        protected ICompression Compression;
        protected ILogger Logger;
        protected ISerializer Serializer;
        protected IAzureQueueConfiguration AzureQueueConfiguration;
        protected ISystemTime SystemTime;
        protected IQueueController Controller;
        protected IMediatorFactory Mediator;

        public QueueControllerTests(ITestOutputHelper output)
        {
            _output = output;
            Setup = NirvanaSetup.Configure()
                .UsingControllerName("ControlelrName", "Root")
                .WithAssembliesFromFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"))
                .SetAdditionalAssemblyNameReferences(new string[0])
                .SetRootTypeAssembly(GetType().Assembly)
                .SetAttributeMatchingFunction(AttributeMatchingFunctionStub)
                .SetDependencyResolver(Resolve, ResolveMany)
                .ForQueries(MediationStrategy.InProcess, MediationStrategy.InProcess)
                .ForCommands(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForInternalEvents(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForUiNotifications(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .BuildConfiguration();

            
            Logger = new TestDebugLogger(output);
            Mediator = new MediatorFactory(Setup);
            Controller = new AzureQueueController(Setup,Logger);
            SystemTime = new SystemTime();
            Compression = new Compression();
            Serializer = new Serializer();
            AzureQueueConfiguration = new AzureQueueConfiguration();
            
            Sut = new AzureQueueController(Setup,new TestDebugLogger(_output));

        }

        private object Resolve(Type arg)
        {
            if (arg == typeof(IQueueFactory))
            {
                return new AzureQueueFactory(AzureQueueConfiguration,Sut,Serializer,SystemTime,Compression,Mediator,Logger );
            }
            return null;
        }
        private object[] ResolveMany(Type arg)
        {
            return new object[0];
        }
    
        [Fact]
        public void should_run_for_5_seconds_and_stop()
        {
            Sut.InitializeAll();
            Sut.AllQueues().All(x=>x.Status == QueueStatus.Started).ShouldEqual(true);

            var stopTime = DateTime.Now.AddSeconds(5);
            var shouldcontinue = true;
            do
            {
                if(stopTime < DateTime.Now)
                Sut.Status= QueueStatus.ShuttingDown;
                var isRunning = Sut.Process();
                shouldcontinue = isRunning;
            } while (shouldcontinue);


            
        }



    }
}