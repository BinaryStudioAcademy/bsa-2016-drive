using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Drive.WebHost.Controllers
{
    [HandleError]
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult InternalServerError()
        {
            var info = new HandleErrorInfo(new Exception(), "Error", "Get");

            Response.StatusCode = 500;
            return View(info);
        }
    }
}