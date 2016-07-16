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
        private readonly ISerializer _serializer;
        private readonly INirvanaConfiguration _endpointConfiguration;

        public WebMediator(ISerializer serializer, INirvanaConfiguration endpointConfiguration)
        {
            _serializer = serializer;
            _endpointConfiguration = endpointConfiguration;
        }




        public string GetQueryApiPath(Type type)
        {
            return CqrsUtils.GetApiEndpint(type, "Query");
        }

        public string GetCommandApiPath(Type type)
        {
            return CqrsUtils.GetApiEndpint(type, "Command");
        }

        public QueryResponse<TResult> Query<TResult>(Query<TResult> query)
        {
            try
            {
                using (var client = new NirvanaNirvanaHttpClient(new HttpClient()))
                {

                    var path = GetQueryApiPath(query.GetType());

                    var uri = new Uri(new Uri(_endpointConfiguration.QueryEndpoint), path);
                    var httpResponseMessage = client.Query(uri.ToString(), query).Result;
                    var response = BuildQueryResponse<TResult>(httpResponseMessage);
                    return QueryResponse.Succeeded(response);
                }
            }
            catch (Exception ex)
            {
                return QueryResponse.Failed<TResult>(ex);
            }

        }

        public CommandResponse<TResult> Command<TResult>(Command<TResult> command)
        {
            try
            {
                using (var client = new NirvanaNirvanaHttpClient(new HttpClient()))
                {

                    var path = GetCommandApiPath(command.GetType());

                    var uri = new Uri(new Uri(_endpointConfiguration.QueryEndpoint), path);
                    var httpResponseMessage = client.Command(uri.ToString(), command).Result;
                    var response = BuildCommandResponse<TResult>(httpResponseMessage);
                    return CommandResponse.Succeeded(response);
                }
            }
            catch (Exception ex)
            {
                return CommandResponse.Failed<TResult>(ex);
            }
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