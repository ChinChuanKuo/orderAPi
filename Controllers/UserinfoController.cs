using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Userinfo")]
    [ApiController]
    [Route("[controller]")]
    public class UserinfoController : Controller
    {
        [HttpGet]
        [Route("searchData")]
        public ActionResult<userModels> searchData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            userModels userModels = new UserinfoClass().GetSearchModels(clientinfo, deviceinfo, clientip);
            if (userModels.client["clientid"].ToString() == "" && userModels.client["accesstoken"].ToString() == "")
                return NotFound();
            return userModels;
        }

        [HttpGet]
        [Route("officeData")]
        public ActionResult<userModels> officeData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            userModels userModels = new UserinfoClass().GetOfficeModels(clientinfo, deviceinfo, clientip);
            if (userModels.client["clientid"].ToString().TrimEnd() == "" && userModels.client["accesstoken"].ToString().TrimEnd() == "")
                return NotFound();
            return userModels;
        }
    }
}