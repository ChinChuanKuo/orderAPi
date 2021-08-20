using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Schedule")]
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : Controller
    {
        [HttpGet]
        [Route("searchData")]
        public List<Dictionary<string, object>> searchData(string clientinfo, string deviceinfo, string dateinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new ScheduleClass().GetSearchModels(clientinfo, deviceinfo, dateinfo, clientip);
        }

    }
}