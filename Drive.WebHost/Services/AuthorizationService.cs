using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Auth;
using System.Configuration;

namespace Drive.WebHost.Services
{
    public class AuthorizationService
    {
        public static DriveService Authorization()
        {
            string[] Scopes = { DriveService.Scope.Drive };
            string ApplicationName = "Drive";

            UserCredential credential;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                          new ClientSecrets
                          {
                              ClientId = ConfigurationManager.AppSettings["CLIENT_ID"],
                              ClientSecret = ConfigurationManager.AppSettings["CLIENT_SECRET"]
                          },
                          new[] { DriveService.Scope.Drive,
                              DriveService.Scope.DriveFile },
                          "user",
                          CancellationToken.None,
                          new FileDataStore(@"c:\datastore", true)).Result;

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
    }
}