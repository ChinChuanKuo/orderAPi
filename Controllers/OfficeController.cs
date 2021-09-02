using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Office")]
    [ApiController]
    [Route("[controller]")]
    public class OfficeController : Controller
    {
        [HttpPost]
        [Route("signinData")]
        public ActionResult<Dictionary<string, object>> signinData([FromForm] string signinfo, [FromForm] string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var officeClass = new OfficeClass().GetSigninModels(signinfo, deviceinfo, clientip);
            if (officeClass.Count == 0)
                return NotFound();
            return officeClass;
        }

        [HttpGet]
        [Route("searchData")]
        public List<Dictionary<string, object>> searchData(string clientinfo, string deviceinfo, string datainfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new OfficeClass().GetSearchModels(clientinfo, deviceinfo, datainfo, clientip);
        }

        [HttpPost]
        [Route("checkData")]
        public Dictionary<string, object> checkData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string datainfo, [FromForm] string infotext)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new OfficeClass().GetCheckModels(clientinfo, deviceinfo, datainfo, infotext, clientip);
        }

        [HttpPost]
        [Route("insertData")]
        public ActionResult<bool> insertData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string officeinfo, [FromForm] string datainfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var officeClass = new OfficeClass().GetInsertModels(clientinfo, deviceinfo, officeinfo, datainfo, clientip);
            if (!officeClass) return NotFound();
            return officeClass;
        }

        [HttpPost]
        [Route("deleteData")]
        public ActionResult<bool> deleteData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string notherinfo, [FromForm] string datainfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var officeClass = new OfficeClass().GetDeleteModels(clientinfo, deviceinfo, notherinfo, datainfo, clientip);
            if (!officeClass) return NotFound();
            return officeClass;
        }
    }
}