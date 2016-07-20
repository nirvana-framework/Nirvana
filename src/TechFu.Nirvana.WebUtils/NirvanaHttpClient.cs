using System.Net.Http;
using System.Threading.Tasks;
using TechFu.Nirvana.CQRS;

namespace TechFu.Nirvana.WebUtils
{
    public class NirvanaHttpClient : INirvanaHttpClient
    {
        public async Task<HttpResponseMessage> Command<T>(string requestUri, Command<T> value)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(requestUri, value).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

        public async Task<HttpResponseMessage> Query<T>(string requestUri, Query<T> value)
        {
            using (var client = new HttpClient())
            {
                var arguments = "";
                var response = await client.GetAsync($"{requestUri}?{arguments}").ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

        public async Task<HttpResponseMessage> UiEvent<T>(string requestUri, UiEvent<T> value)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(requestUri, value).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

      
    }
}