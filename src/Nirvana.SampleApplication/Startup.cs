using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nirvana.Configuration;
using Nirvana.Mediation;
using Nirvana.Sample.Infrastructure.IoC;
using Nirvana.SampleApplication.Services.Domain.Sample.Queries;
using Nirvana.SampleApplication.Services.Services;
using Nirvana.Util;
using Nirvana.Util.Tine;
using Nirvana.Web.Generation;
using Nirvana.Web.Startup;
using StructureMap;

namespace Nirvana.SampleApplication
{
  

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

     

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            new ServicesBuilder().ConfigurePublicCors(app);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
          
            var config = new ApiConfiguration();
        
            var builder = new ServicesBuilder()
                .AddMvc(services)
                .AddCors(services);

            

            var thirdPartyReferences = new[]
            {
                GetType().Assembly,
                typeof(GetSampleDataQuery).Assembly
            };




            //Create our StructureMap container
            var container = new Container();
            container.Configure(c =>
            {


                c.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<GetSampleDataQuery>();
                    scan.WithDefaultConventions();
                    scan.AssemblyContainingType<ISystemTime>();
                    scan.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(IEventHandler<>));


                    scan.AddAllTypesOf<IStartupStep>();
                });

                //Add in our custom registry
                c.AddRegistry(new TestRegistry());
                c.AddRegistry(new NirvanaRegistry());
                //Push the .net Core Services Collection into StructureMap
                c.Populate(services);
            });


            // Finally, make sure we return an IServiceProvider. This makes
            // ASP.NET use the StructureMap container to resolve its services.
            var configureServices = container.GetInstance<IServiceProvider>();
            
            var provider = new InternalProvider(configureServices);

            var setup = NirvanaSetup.Configure()
                .UsingControllerName(config.ControllerAssemblyName, config.RootNamespace)
                .WithAssembliesFromFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"))
                .SetAdditionalAssemblyNameReferences(config.AssemblyNameReferences)
                .SetRootTypeAssembly(typeof(SampleServiceServiceRoot).Assembly)
                .SetDependencyResolver(provider.GetService, provider.GetAllServices)
                .ForQueries(MediationStrategy.InProcess, MediationStrategy.InProcess)
                .ForCommands(MediationStrategy.ForwardLongRunningToQueue, MediationStrategy.ForwardLongRunningToQueue, MediationStrategy.None)
                .ForInternalEvents(MediationStrategy.InProcess, MediationStrategy.InProcess, MediationStrategy.None)
                .ForUiNotifications(MediationStrategy.None, MediationStrategy.None, MediationStrategy.None)
                .BuildConfiguration();
            

            container.Configure(x =>
            {
                x.For<NirvanaSetup>().Singleton().Use(setup);
            });

            var cqrsApiGenerator = new CqrsApiGenerator(setup);
//            var source = cqrsApiGenerator.BuildTree().ToString();
            var assembly = cqrsApiGenerator.LoadAssembly(thirdPartyReferences);
           services.AddMvc()
                .AddApplicationPart(assembly);
            

//            
//// create an assembly part from a class's assembly
//            var assembly = typeof(Startup).GetTypeInfo().Assembly;
//            services.AddMvc()
//                .AddApplicationPart(assembly);
//
//// OR
//            var assembly = typeof(Startup).GetTypeInfo().Assembly;
//            var part = new AssemblyPart(assembly);
//            services.AddMvc()
//                .ConfigureApplicationPartManager(apm => p.ApplicationParts.Add(part));


            return configureServices;
        }

    }
}
