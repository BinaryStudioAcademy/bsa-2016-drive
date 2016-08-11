using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Drive.Identity.Services;

namespace Drive.Core.HttpClient
{
    public class AsyncHttpClient : IAsyncHttpClient
    {
        private readonly BSIdentityManager _identityManager;
        private const string BaseAddress = "http://team.binary-studio.com";
        private readonly IEnumerable<JsonMediaTypeFormatter> _formatter;

        public AsyncHttpClient(BSIdentityManager identityManager)
        {
            _identityManager = identityManager;
            _formatter = PolymorphicMediaTypeFormatter.GetFormatterAsEnumerable();
        }

        public async Task PostAsync<TContent>(string url, TContent content)
        {
            await SendRequest(url, content, (c, u, p) => c.PostAsync(u, p)).ConfigureAwait(false);
        }

        public async Task<TResult> PostAsync<TContent, TResult>(string url, TContent content)
        {
            var response = await SendRequest(url, content, (c, u, p) => c.GetAsync(u)).ConfigureAwait(false);

            var result = await ParseResponseAsync<TResult>(response);
            return result;
        }

        public async Task PutAsync<T>(string url, T content)
        {
            await SendRequest(url, content, (c, u, p) => c.PutAsync(u, p)).ConfigureAwait(false);
        }

        public async Task<TResult> PutAsync<TContent, TResult>(string url, TContent content)
        {
            var response = await SendRequest(url, content, (c, u, p) => c.PutAsync(u, p)).ConfigureAwait(false);
            var result = await ParseResponseAsync<TResult>(response);
            return result;
        }

        public async Task DeleteAsync(string url)
        {
            await SendRequest(url, (c, u, p) => c.DeleteAsync(u)).ConfigureAwait(false);
        }

        public async Task<TResult> DeleteAsync<TResult>(string url)
        {
            var response = await SendRequest(url, (c, u, p) => c.DeleteAsync(u)).ConfigureAwait(false);
            var result = await ParseResponseAsync<TResult>(response);
            return result;
        }

        public async Task<TResult> GetAsync<TResult>(string url)
        {
            var response = await SendRequest(url, (c, u, p) => c.GetAsync(u)).ConfigureAwait(false);

            var result = await ParseResponseAsync<TResult>(response);
            return result;
        }

        private Task<HttpResponseMessage> SendRequest(string url,
            Func<System.Net.Http.HttpClient, string, ObjectContent, Task<HttpResponseMessage>> func)
        {
            return SendRequest(url, (object) null, func);
        }

        private async Task<HttpResponseMessage> SendRequest<TContent>(string url, TContent content,
            Func<System.Net.Http.HttpClient, string, ObjectContent, Task<HttpResponseMessage>> func)
        {
            HttpResponseMessage response = null;
            try
            {
                var baseAddress = new Uri(BaseAddress);
                var cookieContainer = new CookieContainer();
                using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
                using (var client = new System.Net.Http.HttpClient(handler) {BaseAddress = baseAddress})
                {
                    var payload = CreatePayload(content);
                    cookieContainer.Add(baseAddress, new Cookie("x-access-token", _identityManager.Token));

                    response = await func(client, url, payload);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                Trace.Write(ex);
            }
                
            return response;
        }

        private static ObjectContent CreatePayload<TContent>(TContent content)
        {
            if (content == null)
                return null;
            var body = new ObjectContent<TContent>(content, PolymorphicMediaTypeFormatter.GetFormatter());
            return body;
        }

        private async Task<T> ParseResponseAsync<T>(HttpResponseMessage response)
        {
            if (response == null)
                return default(T);

            try
            {
                var result = default(T);
                if (response.Content != null)
                    result = await response.Content.ReadAsAsync<T>(_formatter).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                Trace.Write(ex);
                return default(T);
            }
            finally
            {
                response.Dispose();
            }
        }
    }
}