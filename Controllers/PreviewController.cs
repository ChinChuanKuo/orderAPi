using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Preview")]
    [ApiController]
    [Route("[controller]")]

    public class PreviewController : Controller
    {
        [HttpGet]
        [Route("searchData")]
        public Dictionary<string, object> searchData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new PreviewClass().GetSearchModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("statistData")]
        public Dictionary<string, object> statistData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new PreviewClass().GetStatistModels(clientinfo, deviceinfo, clientip);
        }
    }
}