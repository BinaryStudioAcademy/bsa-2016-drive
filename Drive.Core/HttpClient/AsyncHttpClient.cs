using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Core.HttpClient
{
    public class AsyncHttpClient : IHttpClient, IDisposable
    {
        private readonly System.Net.Http.HttpClient _client;

        public AsyncHttpClient()
        {
            _client = new System.Net.Http.HttpClient();
        }

        public async Task<string> GetAsync(string url)
        {
            var response = await _client.GetAsync(url);

            return await ProcessResult(response);
        }

        public async Task<string> PostAsync(string url, string content)
        {
            var response = await _client.PostAsync(url, new StringContent(content));

            return await ProcessResult(response);
        }

        public async Task<string> PutAsync(string url, string content)
        {
            var response = await _client.PutAsync(url, new StringContent(content));

            return await ProcessResult(response);
        }

        public async Task<string> DeleteAsync(string url)
        {
            var response = await _client.DeleteAsync(url);
            return await ProcessResult(response);
        }

        private static async Task<string> ProcessResult(HttpResponseMessage response)
        {
            var code = (int)response.StatusCode;

            if (code == 200)
            {
                return await response.Content.ReadAsStringAsync();
            }
            if (code >= 400 && code < 500)
            {
                return null;
            }
            throw new HttpRequestException(response.StatusCode.ToString());
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }

                disposedValue = true;
            }
        }

        // ~AsyncHttpClient() {
        //   Dispose(false);
        // }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
