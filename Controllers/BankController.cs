using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Bank")]
    [ApiController]
    [Route("[controller]")]

    public class BankController : Controller
    {
        [HttpGet]
        [Route("balanceData")]
        public Dictionary<string, object> balanceData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new BankClass().GetBalenceModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("searchData")]
        public List<Dictionary<string, object>> searchData(string clientinfo, string deviceinfo, string dateinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new BankClass().GetSearchModels(clientinfo, deviceinfo, dateinfo, clientip);
        }

        [HttpGet]
        [Route("statistData")]
        public List<Dictionary<string, object>> statistData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new BankClass().GetStatistModels(clientinfo, deviceinfo, clientip);
        }

        [HttpPost]
        [Route("checkData")]
        public ActionResult<Dictionary<string, object>> accessData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string infotext)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var bankClass = new BankClass().GetCheckModels(clientinfo, deviceinfo, infotext, clientip);
            if (bankClass.Count == 0)
                return NotFound();
            return bankClass;
        }

        [HttpPost]
        [Route("insertData")]
        public ActionResult<Dictionary<string, object>> insertData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string notherinfo, [FromForm] string storetext)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var bankClass = new BankClass().GetInsertModels(clientinfo, deviceinfo, notherinfo, storetext, clientip);
            if (bankClass.Count == 0)
                return NotFound();
            return bankClass;
        }
    }
}