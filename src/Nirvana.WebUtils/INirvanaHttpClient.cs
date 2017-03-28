using System.Net.Http;
using System.Threading.Tasks;
using Nirvana.CQRS;

namespace Nirvana.WebUtils
{
    public interface INirvanaHttpClient
    {
        Task<HttpResponseMessage> Command<T>(string requestUri, Command<T> value);
        Task<HttpResponseMessage> Query<T>(string requestUrl, Query<T> value);
        Task<HttpResponseMessage> UiEvent<T>(string requestUrl, UiNotification<T> value);
        Task<HttpResponseMessage> InternalEvent(string requestUrl, InternalEvent value);
    }
}