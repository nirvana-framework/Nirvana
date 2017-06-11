using System.Net.Http;
using System.Threading.Tasks;
using Nirvana.CQRS;
using Nirvana.Logging;
using Nirvana.Util.Io;

namespace Nirvana.WebUtils
{
    public class NirvanaHttpClient : INirvanaHttpClient
    {
        private readonly ILogger _logger;
        private readonly ISerializer  _serializer;

        public NirvanaHttpClient(ILogger logger, ISerializer serializer)
        {
            _logger = logger;
            _serializer = serializer;
        }

        public HttpResponseMessage Command<T>(string requestUri, Command<T> value)
        {
            if (_logger.LogDetailedDebug)
            {
                _logger.DetailedDebug($"Url: {requestUri}, Payload:{_serializer.Serialize(value)}");

            }
            using (var client = new HttpClient())
            {
               
                var response = client.PostAsJsonAsync(requestUri, value);
                response.Wait();
                response.Result.EnsureSuccessStatusCode();
                return response.Result;
            }
        }
        public HttpResponseMessage InternalEvent(string requestUri, InternalEvent value)
        {
            if (_logger.LogDetailedDebug)
            {
                _logger.DetailedDebug($"Url: {requestUri}, Payload:{_serializer.Serialize(value)}");

            }
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(requestUri, value);
                response.Wait();
                return response.Result;
            }
        }

        public HttpResponseMessage Query<T>(string requestUri, Query<T> value)
        {

            if (_logger.LogDetailedDebug)
            {
                _logger.DetailedDebug($"Url: {requestUri}, Payload:{_serializer.Serialize(value)}");

            }

            using (var client = new HttpClient())
            {
                var arguments = "";
                var response = client.GetAsync($"{requestUri}?{arguments}");
                response.Wait();

                return response.Result;
            }
        }

        public HttpResponseMessage UiEvent<T>(string requestUri, UiNotification<T> value)
        {
            if (_logger.LogDetailedDebug)
            {
                _logger.DetailedDebug($"Url: {requestUri}, Payload:{_serializer.Serialize(value)}");

            }

            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(requestUri, value);
                response.Wait();
                return response.Result;
            }
        }

      
    }
}