using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Nirvana.CQRS;
using Nirvana.Logging;
using Nirvana.Util.Io;

namespace Nirvana.WebUtils
{

    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data,ISerializer serializer)
        {
            var dataAsString = serializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content,ISerializer serializer)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return serializer.Deserialize<T>(dataAsString);
        }
    }


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
            if (_logger.LogDebug)
            {
                _logger.Debug($"Url: {requestUri}, Payload:{_serializer.Serialize(value)}");

            }
            using (var client = new HttpClient())
            {
               
                var response = client.PostAsJsonAsync(requestUri, value,_serializer);
                response.Wait();
                response.Result.EnsureSuccessStatusCode();
                return response.Result;
            }
        }
        public HttpResponseMessage InternalEvent(string requestUri, InternalEvent value)
        {
            if (_logger.LogDebug)
            {
                _logger.Debug($"Url: {requestUri}, Payload:{_serializer.Serialize(value)}");

            }
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(requestUri, value,_serializer);
                response.Wait();
                return response.Result;
            }
        }

        public HttpResponseMessage Query<T>(string requestUri, Query<T> value)
        {
            if (_logger.LogDebug)
            {
                _logger.Debug($"Url: {requestUri}, Payload:{_serializer.Serialize(value)}");

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
            if (_logger.LogDebug)
            {
                _logger.Debug($"Url: {requestUri}, Payload:{_serializer.Serialize(value)}");

            }

            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(requestUri, value,_serializer);
                response.Wait();
                return response.Result;
            }
        }

      
    }
}