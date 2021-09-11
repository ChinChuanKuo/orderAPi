using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Menu")]
    [ApiController]
    [Route("[controller]")]
    public class MenuController : Controller
    {
        [HttpGet]
        [Route("officeData")]
        public Dictionary<string, object> officeData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new MenuClass().GetOfficeModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("searchData")]
        public Dictionary<string, object> searchData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new MenuClass().GetSearchModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("orderData")]
        public List<Dictionary<string, object>> orderData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new MenuClass().GetOrderModels(clientinfo, deviceinfo, clientip);
        }

        [HttpPost]
        [Route("insertData")]
        public ActionResult<Dictionary<string, object>> insertData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string menuinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var menuClass = new MenuClass().GetInsertModels(clientinfo, deviceinfo, menuinfo, clientip);
            if (menuClass.Count == 0) return NotFound();
            return menuClass;
        }

        [HttpPost]
        [Route("deleteData")]
        public ActionResult<Dictionary<string, object>> deleteData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var menuClass = new MenuClass().GetDeleteModels(clientinfo, deviceinfo, requiredinfo, clientip);
            if (menuClass.Count == 0) return NotFound();
            return menuClass;
        }
    }
}