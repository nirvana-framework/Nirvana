using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.WebUtils
{
    public interface INirvanaHttpClient
    {
        Task<HttpResponseMessage> Command<T>(string requestUri, Command<T> value);
        Task<HttpResponseMessage> Query<T>(string requestUrl, Query<T> value);
    }

    public class NirvanaNirvanaHttpClient : INirvanaHttpClient, IDisposable
    {
        private HttpClient _httpClient;

        public NirvanaNirvanaHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~NirvanaNirvanaHttpClient()
        {
            Dispose(false);
        }


        public async Task<HttpResponseMessage> Command<T>(string requestUri, Command<T> value) 
        {
            var response = await _httpClient.PostAsJsonAsync(requestUri, value);
            response.EnsureSuccessStatusCode();
            return response;
        }


        public async Task<HttpResponseMessage> Query<T>(string requestUri, Query<T> value)
        {
            var arguments = "";

            var response = await _httpClient.GetAsync($"{requestUri}?{arguments}");
            response.EnsureSuccessStatusCode();
            return response;
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }
    }
}