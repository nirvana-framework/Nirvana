using System;
using System.Net.Http;
using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.CQRS.Util;
using Nirvana.Mediation;
using Nirvana.Util.Io;

namespace Nirvana.WebUtils
{
    public class WebMediator : IWebMediator
    {
        private readonly INirvanaConfiguration _endpointConfiguration;
        private readonly INirvanaHttpClient _httpClient;
        private readonly ISerializer _serializer;


        private readonly NirvanaSetup _setup;

        public WebMediator(NirvanaSetup setup)
        {
            _setup = setup;
        }

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
                return QueryResponse.Success(response);
            }
            catch (Exception ex)
            {
                return QueryResponse.Fail<TResult>(ex);
            }
        }

        public UIEventResponse UiNotification<T>(UiEvent<T> uiEevent)
        {
            try
            {
                var path = GetUiNotificationPath(uiEevent.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.NotificationEndpoint), path);
                var httpResponseMessage = _httpClient.UiEvent(uri.ToString(), uiEevent).Result;
                return UIEventResponse.Succeeded();
            }
            catch (Exception)
            {
                return UIEventResponse.Failed();
            }
        }

        public InternalEventResponse InternalEvent(InternalEvent internalEvent)
        {
            try
            {
                var path = GetInternalEventApiPath(internalEvent.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.InternalEventEndpoint), path);
                var httpResponseMessage = _httpClient.InternalEvent(uri.ToString(), internalEvent).Result;
                var response = BuildInternalEventResponse(httpResponseMessage);
                return response;
            }
            catch (Exception)
            {
                return InternalEventResponse.Failed();
            }
        }


        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            try
            {
                var path = GetCommandApiPath(command.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.CommandEndpoint), path);
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
            return _setup.GetApiEndpint(type, "Query");
        }

        public string GetInternalEventApiPath(Type type)
        {
            return _setup.GetApiEndpint(type, "");
        }

        public string GetCommandApiPath(Type type)
        {
            return _setup.GetApiEndpint(type, "Command");
        }

        public string GetUiNotificationPath(Type type)
        {
            //Use one hub for all tasks for now...
            var rootTypeName = "UiNotifications";
            return $"{rootTypeName}/{type.Name.Replace("UiEvent", "")}";
        }

        private T BuildCommandResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            return _serializer.Deserialize<T>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        private InternalEventResponse BuildInternalEventResponse(HttpResponseMessage httpResponseMessage)
        {
            return _serializer.Deserialize<InternalEventResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        private T BuildQueryResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            return _serializer.Deserialize<T>(httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}