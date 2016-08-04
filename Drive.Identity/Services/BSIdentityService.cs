using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using Drive.Core.HttpClient;
using Drive.Identity.Entities;
using Drive.Identity.Errors;
using Drive.Identity.Services.Abstract;

namespace Drive.Identity.Services
{
    public class BSIdentityService : IIdentityService
    {
        private readonly IHttpClient _client;
        private string _baseUrl = "https://bsauth.herokuapp.com";

        public BSIdentityService(IHttpClient client)
        {
            _client = client;
        }

        public async Task<IIdentity> GetIdentityAsync(string login, string password)
        {
            var postData = $"email={login}&password={password}";
            var requestUrl = _baseUrl + "/api/login";
            try
            {
                var result = await _client.PostAsync(requestUrl, postData);
                if (result == null) throw new AuthException("Empty response from auth service");

                var deserializedResponse = BSAuthResponceSerializer.Deserialize(result);
                if (!deserializedResponse.Success) throw new AuthException(deserializedResponse.Message);

                return new BSIdentity {Token = deserializedResponse.Token};
            }
            catch (HttpRequestException ex)
            {
                throw new AuthRequestException("server error", ex);
            }
            
        }
    }
}
