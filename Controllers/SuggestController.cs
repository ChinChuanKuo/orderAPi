using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Suggest")]
    [ApiController]
    [Route("[controller]")]

    public class SuggestController : Controller
    {
        [HttpGet]
        [Route("searchData")]
        public List<Dictionary<string, object>> searchData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new SuggestClass().GetSearchModels(clientinfo, deviceinfo, clientip);
        }

        [HttpPost]
        [Route("insertData")]
        public ActionResult<bool> insertData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string shopinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var SuggestClass = new SuggestClass().GetInsertModels(clientinfo, deviceinfo, shopinfo, clientip);
            if (!SuggestClass) return NotFound();
            return SuggestClass;
        }

        [HttpPost]
        [Route("deleteData")]
        public ActionResult<bool> deleteData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var SuggestClass = new SuggestClass().GetDeleteModels(clientinfo, deviceinfo, requiredinfo, clientip);
            if (!SuggestClass) return NotFound();
            return SuggestClass;
        }

        [HttpPost]
        [Route("createData")]
        public ActionResult<Dictionary<string, object>> createData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var SuggestClass = new SuggestClass().GetCreateModels(clientinfo, deviceinfo, requiredinfo, clientip);
            if (SuggestClass.Count == 0) return NotFound();
            return SuggestClass;
        }
    }
}