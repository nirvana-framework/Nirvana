using System.Net.Http;
using System.Threading.Tasks;
using Nirvana.CQRS;

namespace Nirvana.WebUtils
{
    public class NirvanaHttpClient : INirvanaHttpClient
    {
        public HttpResponseMessage Command<T>(string requestUri, Command<T> value)
        {
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
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(requestUri, value);
                response.Wait();
                return response.Result;
            }
        }

        public HttpResponseMessage Query<T>(string requestUri, Query<T> value)
        {
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
            using (var client = new HttpClient())
            {
                var response = client.PostAsJsonAsync(requestUri, value);
                response.Wait();
                return response.Result;
            }
        }

      
    }
}