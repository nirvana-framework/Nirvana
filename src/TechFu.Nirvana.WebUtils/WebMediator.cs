using System;
using System.Net.Http;
using TechFu.Nirvana.Configuration;
using TechFu.Nirvana.CQRS;
using TechFu.Nirvana.CQRS.Util;
using TechFu.Nirvana.Mediation;
using TechFu.Nirvana.Util.Io;

namespace TechFu.Nirvana.WebUtils
{
    public class WebMediator : IWebMediator
    {
        private readonly INirvanaConfiguration _endpointConfiguration;
        private readonly INirvanaHttpClient _httpClient;
        private readonly ISerializer _serializer;

        public WebMediator(ISerializer serializer, INirvanaConfiguration endpointConfiguration,
            INirvanaHttpClient httpClient)
        {
            _serializer = serializer;
            _endpointConfiguration = endpointConfiguration;
            _httpClient = httpClient;
        }

        public QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            try
            {
                var path = GetQueryApiPath(query.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.QueryEndpoint), path);
                var httpResponseMessage = _httpClient.Query(uri.ToString(), query).Result;
                var response = BuildQueryResponse<TResult>(httpResponseMessage);
                return QueryResponse.Succeeded(response);
            }
            catch (Exception ex)
            {
                return QueryResponse.Failed<TResult>(ex);
            }
        }

        public UIEventResponse UiNotification<T>(UiEvent<T> uiEevent)
        {
            try
            {
                var path = GetCommandApiPath(uiEevent.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.NotificationEndpoint), path);
                var httpResponseMessage = _httpClient.UiEvent(uri.ToString(), uiEevent).Result;
                return UIEventResponse.Succeeded();
            }
            catch (Exception ex)
            {
                return UIEventResponse.Failed();
            }
        }

        public InternalEventResponse InternalEvent<T>(InternalEvent<T> internalEvent)
        {
            throw new NotImplementedException();
        }


        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            try
            {
                var path = GetCommandApiPath(command.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.QueryEndpoint), path);
                var httpResponseMessage = _httpClient.Command(uri.ToString(), command).Result;
                var response = BuildCommandResponse<TResult>(httpResponseMessage);
                return CommandResponse.Succeeded(response);
            }
            catch (Exception ex)
            {
                return CommandResponse.Failed<TResult>(ex);
            }
        }

        public string GetQueryApiPath(Type type)
        {
            return CqrsUtils.GetApiEndpint(type, "Query");
        }

        public string GetCommandApiPath(Type type)
        {
            return CqrsUtils.GetApiEndpint(type, "Command");
        }

        private T BuildCommandResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            return _serializer.Deserialize<T>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        private T BuildQueryResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            return _serializer.Deserialize<T>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}