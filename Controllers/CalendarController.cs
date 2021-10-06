using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Calendar")]
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : Controller
    {
        [HttpGet]
        [Route("searchData")]
        public List<Dictionary<string, object>> searchData(string clientinfo, string deviceinfo, string dateinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new CalendarClass().GetSearchModels(clientinfo, deviceinfo, dateinfo, clientip);
        }

        [HttpGet]
        [Route("categoryData")]
        public List<Dictionary<string, object>> categoryData(string clientinfo, string deviceinfo, string datainfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new CalendarClass().GetCategoryModels(clientinfo, deviceinfo, datainfo, clientip);
        }

        [HttpPost]
        [Route("deleteData")]
        public ActionResult<bool> deleteData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo, [FromForm] string datainfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var calendarClass = new CalendarClass().GetDeleteModels(clientinfo, deviceinfo, requiredinfo, datainfo, clientip);
            if (!calendarClass) return NotFound();
            return calendarClass;
        }

        [HttpPost]
        [Route("modifyData")]
        public ActionResult<Dictionary<string, object>> modifyData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo, [FromForm] string categoryinfo, [FromForm] string datainfo, [FromForm] string timeinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var calendarClass = new CalendarClass().GetModifyModels(clientinfo, deviceinfo, requiredinfo, categoryinfo, datainfo, timeinfo, clientip);
            if (calendarClass.Count == 0) return NotFound();
            return calendarClass;
        }

        [HttpPost]
        [Route("createData")]
        public ActionResult<Dictionary<string, object>> createData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string categoryinfo, [FromForm] string dateinfo, [FromForm] string timeinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var calendarClass = new CalendarClass().GetCreateModels(clientinfo, deviceinfo, categoryinfo, dateinfo, timeinfo, clientip);
            if (calendarClass.Count == 0) return NotFound();
            return calendarClass;
        }
    }
}