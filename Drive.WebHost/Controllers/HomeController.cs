using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drive.DataAccess.Context;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Drive.DataAccess.Repositories;
using Drive.Logging;

namespace Drive.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogging _logging;

        public HomeController(IUnitOfWork unitOfWork, ILogging logging)
        {
            _logging = logging;
            unitOfWork.Users.Create(new User()
            {
                Email = "s@s.s"
            });
            unitOfWork.SaveChanges();


            _logging.Write("test log");

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}