using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.WebApi
{
    public class DynamicApiSelector : DefaultHttpControllerSelector
    {
        private static readonly Dictionary<string, Type> _handledControllers;
        private static string  _baseNamespace;
        private readonly HttpConfiguration _configuration;

        private readonly string ApiDllName;
        private readonly Type[] _inlineControllerTypes;
        static DynamicApiSelector()
        {
            _handledControllers = new Dictionary<string, Type>();
        }

        public DynamicApiSelector(HttpConfiguration configuration,Type[] inlineControllerTypes,string dynamidDllName, string baseControllerNamespace) : base(configuration)
        {
            ApiDllName = dynamidDllName;
            this._inlineControllerTypes = inlineControllerTypes;
            _baseNamespace = baseControllerNamespace;
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            
            var controllerName = GetControllerName(request);
            var inlineType = _inlineControllerTypes.FirstOrDefault(x => StringExtensions.EqualsIgnoreCase(x.Name, controllerName + "Controller"));
            if (inlineType!=null)
            {
                return new HttpControllerDescriptor(_configuration, controllerName,inlineType);
            }
            var rootType =  Enum.Parse(NirvanaConfigSettings.Configuration.RootType, controllerName,true).ToString();
            if (!_handledControllers.ContainsKey(rootType))
            {
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (a.GetName().Name == ApiDllName)
                        foreach (var t in a.GetTypes())
                        {
                            if (t.FullName.EqualsIgnoreCase($"{_baseNamespace}.Controllers.{controllerName}Controller"))
                            {
                                _handledControllers[rootType] = t;
                            }
                        }
                }
            }


            return new HttpControllerDescriptor(_configuration, controllerName, _handledControllers[rootType]);
        }
    }
}