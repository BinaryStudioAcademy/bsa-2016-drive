﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Drive.WebHost.Services;

namespace Drive.WebHost.Api
{
    [RoutePrefix("api/logs")]
    public class LogsController : ApiController
    {
        private readonly ILogsService _logService;

        public LogsController(ILogsService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var result = await _logService.GetAllAsync();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            var result = await _logService.GetAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(int id)
        {
            await _logService.DeleteAsync(id);
            return Ok();
        }
    }
}