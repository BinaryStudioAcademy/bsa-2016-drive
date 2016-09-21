using System.Web.Mvc;
using System.Threading.Tasks;
using Drive.WebHost.Services;
using System;

namespace Drive.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISharedByLinkService _sharedByLinkService;
        public HomeController(ISharedByLinkService service)
        {
            _sharedByLinkService = service;
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
    }
}