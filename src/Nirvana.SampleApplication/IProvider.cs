using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Nirvana.SampleApplication
{
    public interface IProvider
    {

        object[] GetAllServices(Type serviceType);
        object GetService(Type serviceType) ;
    }


    public class InternalProvider:IProvider
    {
        private readonly IServiceProvider _provider;

        public InternalProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public object[] GetAllServices(Type serviceType)
            => _provider.GetServices(serviceType).ToArray();

        
        public object GetService(Type serviceType) => _provider.GetService(serviceType);

    }
}