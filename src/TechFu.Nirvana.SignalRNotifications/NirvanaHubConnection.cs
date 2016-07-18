using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNet.SignalR.Client;
using TechFu.Nirvana.Util.Extensions;

namespace TechFu.Nirvana.SignalRNotifications
{
    public abstract class NirvanaHubConnection<TRootAttributType> : HubConnection, IUiINotificationHub<TRootAttributType>
    {
        protected const int ConnectTimeout = 30000; // 30s
        protected const int InvokeRetryCount = 4; // attempts
        protected const int InvokeMaxDelay = 15; // seconds

        private readonly Dictionary<string, IHubProxy> _hubProxies = new Dictionary<string, IHubProxy>();

        private readonly object _initializeLock;

        private bool _initialized;

        public abstract Type[] Hubs { get; }

        protected NirvanaHubConnection(string url) : base(url)
        {
            _initialized = false;
            _initializeLock = new object();
        }


        private void Initialize()
        {
            lock (_initializeLock)
            {
                if (_initialized)
                    return;
                var hubProxyNames = Hubs.Select(t => ConvertInterfaceIntoHubName(t.Name));
                _hubProxies.Clear();
                hubProxyNames.ForEach(n => { _hubProxies.Add(n, CreateHubProxy(n)); });
                _initialized = true;
            }
        }


        private void Connect()
        {
            if (State == ConnectionState.Disconnected)
            {
                Start().Wait(ConnectTimeout);

                if (State != ConnectionState.Connected)
                    throw new InvalidOperationException("Could not connect");
            }
        }

        private void Invoke(string hubName, string method, params object[] args)
        {
            Initialize();

            IHubProxy proxy;
            if (!_hubProxies.TryGetValue(hubName, out proxy))
                throw new InvalidOperationException("Could not find hub " + hubName);


            try
            {
                Connect();
                var i = proxy.Invoke(method, args);
                i.Wait();
            }
            catch (Exception e)
            {
                Stop(e);
            }
        }

        public void Invoke<T>(Expression<Action<T>> method)
        {
            var hubTypeName = typeof(T).Name;
            var hubName = ConvertInterfaceIntoHubName(hubTypeName);

            var body = method.Body as MethodCallExpression;
            if (body == null)
                throw new ArgumentException("Expression must be a method call");

            if (body.Object != method.Parameters[0])
                throw new ArgumentException("Method call must target lambda argument");

            var methodName = body.Method.Name.ToCamelCase();
            var arguments = body.Arguments.Select(a =>
            {
                var lambda = Expression.Lambda(a, method.Parameters);
                var d = lambda.Compile();

                return d.DynamicInvoke(new object[1]);
            }).ToArray();

            Invoke(hubName, methodName, arguments);
        }

        private static string ConvertInterfaceIntoHubName(string hubName)
        {
            return hubName.Substring(1, hubName.Length - 1).ToCamelCase();
        }
    }

    public interface IUiINotificationHub<TRootAttributType>
    {
        void Invoke<T>(Expression<Action<T>> method);
    }
}