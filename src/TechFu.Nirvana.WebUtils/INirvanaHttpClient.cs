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
        Task<HttpResponseMessage> UiEvent<T>(string requestUrl, UiEvent<T> value);
        Task<HttpResponseMessage> InternalEvent(string requestUrl, InternalEvent value);
    }
}