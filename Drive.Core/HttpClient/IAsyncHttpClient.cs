using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Core.HttpClient
{
    public interface IAsyncHttpClient
    {
        Task PostAsync<TContent>(string url, TContent content);
        Task<TResult> PostAsync<TContent, TResult>(string url, TContent content);
        Task PutAsync<T>(string url, T content);
        Task<TResult> PutAsync<TContent, TResult>(string url, TContent content);
        Task DeleteAsync(string url);
        Task<TResult> DeleteAsync<TResult>(string url);
        Task<TResult> GetAsync<TResult>(string url);
    }
}
