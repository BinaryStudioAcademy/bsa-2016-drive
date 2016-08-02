using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        [HttpDelete]
        public void Delete(int id)
        {
            
        }
    }
}
