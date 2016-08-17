using System.Web.Mvc;
using Drive.WebHost.Filters;
using Drive.WebHost.Services;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace Drive.WebHost.Infrastructure
{
    public class WebHostModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISpaceService>().To<SpaceService>();
            Kernel.Bind<IFolderService>().To<FolderService>();

            Kernel.Bind<ILogsService>().To<LogsService>();

            Kernel.Bind<IUsersService>().To<UsersService>();

            Kernel.Bind<IUsersProvider>().To<UsersProvider>();

            Kernel.Bind<IFileService>().To<FileService>();

            //Kernel.BindFilter<JWTAuthenticationFilter>(FilterScope.Global, 0);
        }
    }
}