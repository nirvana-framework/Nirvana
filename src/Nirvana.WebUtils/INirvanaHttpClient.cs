using System.Net.Http;
using System.Threading.Tasks;
using Nirvana.CQRS;

namespace Nirvana.WebUtils
{
    public interface INirvanaHttpClient
    {
        HttpResponseMessage Command<T>(string requestUri, Command<T> value);
        HttpResponseMessage Query<T>(string requestUrl, Query<T> value);
        HttpResponseMessage UiEvent<T>(string requestUrl, UiNotification<T> value);
        HttpResponseMessage InternalEvent(string requestUrl, InternalEvent value);
    }
}