using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Review")]
    [ApiController]
    [Route("[controller]")]

    public class ReviewController : Controller
    {
        [HttpGet]
        [Route("searchData")]
        public Dictionary<string, object> searchData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new ReviewClass().GetSearchModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("statistData")]
        public Dictionary<string, object> statistData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new ReviewClass().GetStatistModels(clientinfo, deviceinfo, clientip);
        }
    }
}