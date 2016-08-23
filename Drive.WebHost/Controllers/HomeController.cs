using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drive.DataAccess.Context;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Drive.DataAccess.Repositories;

namespace Drive.WebHost.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.BasePath = System.Configuration.ConfigurationManager.AppSettings["basePath"];
            return View();
        }
    }
}