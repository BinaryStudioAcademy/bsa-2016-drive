using Drive.WebHost.Services;
using Ninject.Modules;

namespace Drive.WebHost.Infrastructure
{
    public class WebHostModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISpaceService>().To<SpaceService>();

            Kernel.Bind<IFolderService>().To<FolderService>();
        }
    }
}