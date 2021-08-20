using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Profile")]
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        [HttpGet]
        [Route("searchData")]
        public ActionResult<Dictionary<string, object>> searchData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            Dictionary<string, object> searchModels = new ProfileClass().GetSearchModels(clientinfo, deviceinfo, clientip);
            if (searchModels.Count == 0)
                return NotFound();
            return searchModels;
        }
    }
}