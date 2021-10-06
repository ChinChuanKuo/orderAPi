using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using orderAPi.Models;

namespace orderAPi.Controllers
{
    [EnableCors("Shop")]
    [ApiController]
    [Route("[controller]")]
    public class ShopController : Controller
    {
        [HttpPost]
        [Route("chooseData")]
        public Dictionary<string, object> chooseData([FromForm] string clientinfo, [FromForm] string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new ShopClass().GetChooseModels(clientinfo, deviceinfo, clientip);
        }

        [HttpPost]
        [Route("searchData")]
        public List<Dictionary<string, object>> searchData([FromForm] string clientinfo, [FromForm] string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new ShopClass().GetSearchModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("statistData")]
        public List<Dictionary<string, object>> statistData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new ShopClass().GetStatistModels(clientinfo, deviceinfo, clientip);
        }

        [HttpGet]
        [Route("categoryData")]
        public List<Dictionary<string, object>> categoryData(string clientinfo, string deviceinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new ShopClass().GetCategoryModels(clientinfo, deviceinfo, clientip);
        }

        [HttpPost]
        [Route("closedData")]
        public ActionResult<Dictionary<string, object>> closedData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var shopClass = new ShopClass().GetClosedModels(clientinfo, deviceinfo, requiredinfo, clientip);
            if (shopClass.Count == 0) return NotFound();
            return shopClass;
        }

        [HttpPost]
        [Route("openedData")]
        public ActionResult<Dictionary<string, object>> openedData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo, [FromForm] string timeinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var shopClass = new ShopClass().GetOpenedModels(clientinfo, deviceinfo, requiredinfo, timeinfo, clientip);
            if (shopClass.Count == 0) return NotFound();
            return shopClass;
        }


        [HttpPost]
        [Route("waitedData")]
        public ActionResult<Dictionary<string, object>> waitedData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo, [FromForm] string timeinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var shopClass = new ShopClass().GetWaitedModels(clientinfo, deviceinfo, requiredinfo, timeinfo, clientip);
            if (shopClass.Count == 0) return NotFound();
            return shopClass;
        }

        [HttpPost]
        [Route("restartData")]
        public ActionResult<Dictionary<string, object>> restartData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var shopClass = new ShopClass().GetRestartModels(clientinfo, deviceinfo, requiredinfo, clientip);
            if (shopClass.Count == 0) return NotFound();
            return shopClass;
        }


        [HttpPost]
        [Route("deleteData")]
        public ActionResult<bool> deleteData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            bool shopClass = new ShopClass().GetDeleteModels(clientinfo, deviceinfo, requiredinfo, clientip);
            if (!shopClass) return NotFound();
            return shopClass;
        }

        [HttpPost]
        [Route("modifyData")]
        public ActionResult<Dictionary<string, object>> modifyData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo, [FromForm] string iteminfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var shopClass = new ShopClass().GetModifyModels(clientinfo, deviceinfo, requiredinfo, iteminfo, clientip);
            if (shopClass.Count == 0) return NotFound();
            return shopClass;
        }

        [HttpPost]
        [Route("insertData")]
        public ActionResult<Dictionary<string, object>> insertData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo, [FromForm] string iteminfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var shopClass = new ShopClass().GetInsertModels(clientinfo, deviceinfo, requiredinfo, iteminfo, clientip);
            if (shopClass.Count == 0) return NotFound();
            return shopClass;
        }

        [HttpPost]
        [Route("createData")]
        public ActionResult<Dictionary<string, object>> createData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string shopinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            var shopClass = new ShopClass().GetCreateModels(clientinfo, deviceinfo, shopinfo, clientip);
            if (shopClass.Count == 0) return NotFound();
            return shopClass;
        }

        [HttpPost]
        [Route("removeData")]
        public ActionResult<bool> removeData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            bool shopClass = new ShopClass().GetRemoveModels(clientinfo, deviceinfo, requiredinfo, clientip);
            if (!shopClass) return NotFound();
            return shopClass;
        }

        [HttpPost]
        [Route("increaseData")]
        public ActionResult<Dictionary<string, object>> increaseData([FromForm] string clientinfo, [FromForm] string deviceinfo, [FromForm] string requiredinfo, [FromForm] string iteminfo)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return new ShopClass().GetIncreaseModels(clientinfo, deviceinfo, requiredinfo, iteminfo, clientip);
        }
    }
}