using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Money")]
    [ApiController]
    [Route("[controller]")]
    public class MoneyController : Controller
    {
        [HttpGet]
        [Route("balanceData")]
        public Dictionary<string, object> balanceData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new MoneyClass().GetBalenceModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("searchData")]
        public List<Dictionary<string, object>> searchData(string clientinfo, string deviceinfo, string dateinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new MoneyClass().GetSearchModels(clientinfo, deviceinfo, dateinfo, clientip);
        }

        [HttpGet]
        [Route("statistData")]
        public List<Dictionary<string, object>> statistData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new MoneyClass().GetStatistModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("analysisData")]
        public List<Dictionary<string, object>> analysisData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new MoneyClass().GetAnalysisModels(clientinfo, deviceinfo, clientip);
        }
    }
}