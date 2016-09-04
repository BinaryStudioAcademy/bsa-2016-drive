using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drive.DataAccess.Context;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Drive.DataAccess.Repositories;
using System.Threading.Tasks;
using Drive.WebHost.Services;

namespace Drive.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpaceService _spaceService;
        private readonly IUsersService _userService;
        public HomeController(ISpaceService spaceService, IUsersService userService)
        {
            _spaceService = spaceService;
            _userService = userService;
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.BasePath = System.Configuration.ConfigurationManager.AppSettings["basePath"];
            await _spaceService.CreateUserAndFirstSpaceAsync(_userService.CurrentUserId);
            return View();
        }
    }
}