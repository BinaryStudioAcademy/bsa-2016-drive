using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Threading.Tasks;  
using System.Web.Http.Results; 

namespace Drive.WebHost.Api
{
    public class GlobalExceptionHandler: ExceptionHandler  
    {  
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
    {
        // Access Exception using context.Exception;  
        const string errorMessage = "Inretnal server error(500)";
        var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
            new
            {
                Message = errorMessage
            });
        response.Headers.Add("X-Error", errorMessage);
        context.Result = new ResponseMessageResult(response);
    }
}  
}