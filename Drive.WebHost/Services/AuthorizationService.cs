using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Auth;
using System.Configuration;
using System;
using System.Web;
using System.IO;

namespace Drive.WebHost.Services
{
    public class AuthorizationService
    {
        public static DriveService Authorization()
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DriveService ServiceAccountAuthorization()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive };

            try
            {
                string credPath = HttpContext.Current.Server.MapPath("~/App_Data");
                credPath = Path.Combine(credPath, "Drive-9345df2608d2.json");
                using (var stream = new FileStream(credPath, FileMode.Open, FileAccess.Read))
                {
                    var credential = GoogleCredential.FromStream(stream);
                    credential = credential.CreateScoped(scopes);

                    DriveService service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Drive",
                    });
                    return service;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return null;
            }
        }
    }
}