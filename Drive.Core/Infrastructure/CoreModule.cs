using Drive.Core.HttpClient;
using Ninject.Modules;

namespace Drive.Core.Infrastructure
{
    public class CoreModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAsyncHttpClient>().To<AsyncHttpClient>();
        }
    }
}
