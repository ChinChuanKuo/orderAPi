using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Signin")]
    [ApiController]
    [Route("[controller]")]
    public class SigninController : Controller
    {
        [HttpPost]
        [Route("signinData")]
        public ActionResult<Dictionary<string, object>> signinData([FromForm] string signinfo, [FromForm] string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var signinClass = new SigninClass().GetSigninModels(signinfo, deviceinfo, clientip);
            if (signinClass.Count == 0)
                return NotFound();
            return signinClass;
        }
    }
}