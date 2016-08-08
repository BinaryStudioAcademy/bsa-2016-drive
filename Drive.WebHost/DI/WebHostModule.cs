using Drive.WebHost.Services;
using Ninject.Modules;

namespace Drive.WebHost.DI
{
    public class WebHostModule : NinjectModule
    {
            public override void Load()
            {
                //Bind<IIdentityInterface>().To<IdentityClass>();
                Kernel.Bind<ISpaceService>().To<SpaceService>();
            }
    }
}