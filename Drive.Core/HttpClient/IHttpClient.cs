using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Drive.Core.HttpClient
{
    public interface IHttpClient
    {
        Task<string> GetAsync(string url);
        Task<string> PostAsync(string url, string content);
        Task<string> PutAsync(string url, string content);
        Task<string> DeleteAsync(string url);
    }
}
