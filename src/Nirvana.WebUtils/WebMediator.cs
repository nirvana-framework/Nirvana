using System;
using System.Net.Http;
using Nirvana.Configuration;
using Nirvana.CQRS;
using Nirvana.CQRS.Util;
using Nirvana.Logging;
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
        private readonly ILogger _logger;

        public WebMediator(ISerializer serializer, INirvanaConfiguration endpointConfiguration,
            INirvanaHttpClient httpClient,NirvanaSetup setup,ILogger logger)
        {
            _setup = setup;
            _logger = logger;
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
                _logger.Debug($"Query URL: {uri.AbsoluteUri}");
                var httpResponseMessage = _httpClient.Query(uri.ToString(), query);
                var response = BuildQueryResponse<TResult>(httpResponseMessage);
                return QueryResponse.Success(response);
            }
            catch (Exception ex)
            {
                return QueryResponse.Fail<TResult>(ex);
            }
        }

        public UIEventResponse UiNotification<T>(UiNotification<T> uiEevent)
        {
            try
            {
                var path = GetUiNotificationPath(uiEevent.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.NotificationEndpoint), path);
                _logger.Debug($"Notification URL: {uri.AbsoluteUri}");
                var httpResponseMessage = _httpClient.UiEvent(uri.ToString(), uiEevent);
                return UIEventResponse.Succeeded();
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return UIEventResponse.Failed();
            }
        }

        public InternalEventResponse InternalEvent(InternalEvent internalEvent)
        {
            try
            {
                var path = GetInternalEventApiPath(internalEvent.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.InternalEventEndpoint), path);
                _logger.Debug($"Event URL: {uri.AbsoluteUri}");
                var httpResponseMessage = _httpClient.InternalEvent(uri.ToString(), internalEvent);
                var response = BuildInternalEventResponse(httpResponseMessage);
                return response;
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
                return InternalEventResponse.Failed();
            }
        }


        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            try
            {
                _logger.Debug($"resolving Command URL: {command.GetType()}, {_endpointConfiguration.CommandEndpoint}");
                var path = GetCommandApiPath(command.GetType());
                var uri = new Uri(new Uri(_endpointConfiguration.CommandEndpoint), path);
                _logger.Debug($"Command URL: {uri.AbsoluteUri}");
                var httpResponseMessage = _httpClient.Command(uri.ToString(), command);
                var response = BuildCommandResponse<CommandResponse<TResult>>(httpResponseMessage);
                return response;
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
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
            try
            {
                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                _logger.Debug(result);
                return _serializer.Deserialize<T>(result);
            }
            catch (Exception ex)
            {

                _logger.Exception(ex);
                throw;
            }
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