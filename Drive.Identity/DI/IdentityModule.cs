using Ninject.Modules;
using Drive.Identity.Services;

namespace Drive.Identity.DI
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            Bind<BSIdentityManager>().ToSelf();
        }
    }
}
