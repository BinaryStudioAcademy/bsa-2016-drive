using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Drive.DataAccess.Context;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Interfaces;
using Drive.DataAccess.Repositories;
using Drive.WebHost.DI;

namespace Drive.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private DependencyCollector _collector;
        private IUnitOfWork _unitOfWork;
        public ActionResult Index()
        {

            using (DriveContext dc = new DriveContext())
            {
                _collector = new DependencyCollector(dc);
                _unitOfWork = _collector.UnitOfWork; 
                _unitOfWork.Users.Create(new User()
                {
                    Login = "test",Password = "test",Email = "test@test.com",IsDeleted = false
                });
                var list = _unitOfWork.Users;
                _unitOfWork.SaveChanges();
            }

            return View();
        }
    }
}