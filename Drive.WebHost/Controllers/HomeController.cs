using System.Web.Mvc;
using System.Threading.Tasks;
using Drive.WebHost.Services;
using System;
using Drive.Logging;

namespace Drive.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISharedByLinkService _sharedByLinkService;
        private readonly IFileService _fileService;
        private readonly IFolderService _folderService;
        private readonly ILogger _logger;
        public HomeController(ISharedByLinkService service, IFileService fileService, IFolderService folderService, ILogger logger)
        {
            _sharedByLinkService = service;
            _fileService = fileService;
            _folderService = folderService;
            _logger = logger;
        }
        public ActionResult Index()
        {
            ViewBag.BasePath = System.Configuration.ConfigurationManager.AppSettings["basePath"];
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Share(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                ViewBag.BasePath = System.Configuration.ConfigurationManager.AppSettings["basePath"];
                var contentList = await _sharedByLinkService.GetContentByLink(Id);
                return View(contentList);
            }
            return View("Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> GetFolderContent(string link, int id)
        {
            ViewBag.BasePath = System.Configuration.ConfigurationManager.AppSettings["basePath"];
            var folderContent = await _sharedByLinkService.GetFolderContent(link, id);
            return PartialView("_SharedContent", folderContent);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Download(string fileLink)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileLink))
                {
                    ViewBag.BasePath = System.Configuration.ConfigurationManager.AppSettings["basePath"];
                    var fileStreamContainer = await _fileService.DownloadFile(fileLink);
                    if (fileStreamContainer != null)
                        return File(fileStreamContainer.Content, fileStreamContainer.Type, fileStreamContainer.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex, ex.Message);
            }
            return View("Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> FindCourseByFileId(int fileId)
        {
            if (fileId != 0)
            {
                ViewBag.BasePath = System.Configuration.ConfigurationManager.AppSettings["basePath"];
                var course = await _fileService.SearchCourse(fileId);
                if (course != null)
                {
                    string coursePath = string.Format("{0}{1}{2}", System.Configuration.ConfigurationManager.AppSettings["basePath"], "/#/apps/academy/",course.Id);
                    return Content(string.Format("<script>window.location = '{0}';</script>", coursePath));
                }                
            }
            return View("Error");
        }

        [AllowAnonymous]
        public async Task<ActionResult> FindEventByFileId(int fileId)
        {
            if (fileId != 0)
            {
                var currentEvent = await _fileService.SearchEvent(fileId);
                if (currentEvent != null)
                {
                    string eventPath = string.Format("{0}{1}{2}", System.Configuration.ConfigurationManager.AppSettings["basePath"], "/#/apps/events/", currentEvent.Id);
                    return Content(string.Format("<script>window.location = '{0}';</script>", eventPath));
                }
            }
            return View("Error");
        }
    }
}