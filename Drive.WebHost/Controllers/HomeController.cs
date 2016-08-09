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
using Microsoft.Ajax.Utilities;

namespace Drive.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logging;

        public HomeController(IUnitOfWork unitOfWork, ILogger logging)
        {
            _logging = logging;
            unitOfWork.Users.Create(new User()
            {
                Email = "s@s.s"
            });
            unitOfWork.SaveChanges();

            try
            {
                throw new DivideByZeroException();
            }
            catch (DivideByZeroException e)
            {
                _logging.WriteError(e, "error");
            }

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}