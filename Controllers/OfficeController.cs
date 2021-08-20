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
    }
}