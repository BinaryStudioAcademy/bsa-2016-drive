using System.Web.Mvc;
using Drive.DataAccess.Entities.Pro;
using Drive.WebHost.Filters;
using Drive.WebHost.Services;
using Drive.WebHost.Services.Pro;
using Drive.WebHost.Services.Pro.Abstract;
using Driver.Shared.Dto.Pro;
using Ninject.Modules;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace Drive.WebHost.Infrastructure
{
    public class WebHostModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IRolesService>().To<RolesService>();
            Kernel.Bind<ISpaceService>().To<SpaceService>();
            Kernel.Bind<IFolderService>().To<FolderService>();
            Kernel.Bind<ILogsService>().To<LogsService>();
            Kernel.Bind<IUsersService>().To<UsersService>();
            Kernel.Bind<IUsersProvider>().To<UsersProvider>();

            Kernel.Bind<IFileService>().To<FileService>();

            Kernel.Bind<IAcademyProCourseService>().To<AcademyProCourseService>();
            Kernel.Bind<ILectureService>().To<LectureService>();
            Kernel.Bind<ICodeSamplesService>().To<CodeSamplesService>();
            Kernel.Bind<IContentLinkService>().To<ContentLinkService>();
            Kernel.Bind<ITagsService>().To<TagsService>();

            //Kernel.BindFilter<JWTAuthenticationFilter>(FilterScope.Global, 0);
            Kernel.Bind<ISharedSpaceService>().To<SharedSpaceService>();

            Kernel.BindFilter<JWTAuthenticationFilter>(FilterScope.Global, 0);
        }
    }
}