using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Drive.Core.HttpClient
{
    public class AsyncHttpClient : IHttpClient, IDisposable
    {
        System.Net.Http.HttpClient _client;

        public AsyncHttpClient()
        {

        }

        public async Task<string> GetAsync(string url)
        {       
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            var baseAddress = new Uri("http://team.binary-studio.com");
            cookieContainer.Add(baseAddress, new Cookie("x-access-token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6IjU3N2ExNjY1OTgyOWZlMDUwYWRiM2Y1YyIsImVtYWlsIjoidGVzdGVyX2FAZXhhbXBsZS5jb20iLCJyb2xlIjoiREVWRUxPUEVSIiwiaWF0IjoxNDcwOTA1MjczfQ.2I_Ml5jEfSG0W5czpC5mwoedrQm-uIiy5aFiqW38gRE"));
            var client = new System.Net.Http.HttpClient(handler);
            HttpResponseMessage result;
            client.BaseAddress = baseAddress;
            result = client.GetAsync("/profile/user/filter").Result;
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
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
                    //_client.Dispose();
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
