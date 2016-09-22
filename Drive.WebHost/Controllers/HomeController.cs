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
        private readonly ILogger _logger;
        public HomeController(ISharedByLinkService service, IFileService fs, ILogger logger)
        {
            _sharedByLinkService = service;
            _fileService = fs;
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
            ViewBag.BasePath = System.Configuration.ConfigurationManager.AppSettings["basePath"];
            var contentList = await _sharedByLinkService.GetContentByLink(Id);
            return View(contentList);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Download(string fileLink)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileLink))
                {
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
    }
}