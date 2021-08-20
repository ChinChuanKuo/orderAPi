using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Notifications")]
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : Controller
    {
        [HttpGet]
        [Route("searchData")]
        public List<Dictionary<string, object>> searchData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new NotificationsClass().GetSearchModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("filterData")]
        public List<Dictionary<string, object>> filterData(string clientinfo, string deviceinfo, string lengthinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new NotificationsClass().GetFilterModels(clientinfo, deviceinfo, lengthinfo, clientip);
        }
    }
}