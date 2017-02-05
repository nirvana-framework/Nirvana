using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.WebApi.Startup
{
    public class DynamicApiSelector : DefaultHttpControllerSelector
    {
        private static readonly Dictionary<string, Type> HandledControllers;
        private readonly HttpConfiguration _configuration;
        private readonly Type[] _inlineControllerTypes;
        static DynamicApiSelector()
        {
            HandledControllers = new Dictionary<string, Type>();
        }

        public DynamicApiSelector(HttpConfiguration configuration,Type[] inlineControllerTypes) : base(configuration)
        {
           
            this._inlineControllerTypes = inlineControllerTypes;
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            
            var controllerName = GetControllerName(request);
            var inlineType = _inlineControllerTypes.FirstOrDefault(x => x.Name.EqualsIgnoreCase(controllerName + "Controller"));
            if (inlineType!=null)
            {
                return new HttpControllerDescriptor(_configuration, controllerName,inlineType);
            }

            var rootType = controllerName == NirvanaSetup.UiNotificationHubName 
                ? NirvanaSetup.UiNotificationHubName
                : NirvanaSetup.RootNames.Single(x=>x.Equals(controllerName,StringComparison.CurrentCultureIgnoreCase));

            if (!HandledControllers.ContainsKey(rootType))
            {
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (a.GetName().Name == NirvanaSetup.ControllerAssemblyName)
                        foreach (var t in a.GetTypes())
                        {
                            if (t.FullName.EqualsIgnoreCase($"{NirvanaSetup.ControllerRootNamespace}.Controllers.{controllerName}Controller"))
                            {
                                HandledControllers[rootType] = t;
                            }
                        }
                }
            }


            return new HttpControllerDescriptor(_configuration, controllerName, HandledControllers[rootType]);
        }
    }
}