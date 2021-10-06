using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class ScheduleClass
    {
        public List<Dictionary<string, object>> GetSearchModels(string clientinfo, string deviceinfo, string dateinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var dateJson = JsonSerializer.Deserialize<Dictionary<string, object>>(dateinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@stdate", DateTime.Parse(dateJson["stdate"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
            dbparams.Add(new dbparam("@endate", DateTime.Parse(dateJson["endate"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.scheduleorder @newid,@stdate,@endate;", dbparams).Rows)
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", dr["orderid"].ToString().TrimEnd() } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["shop"].ToString().TrimEnd()) }, { "stdate", new Dictionary<string, object>() { { "data", $"{dr["stdate"].ToString().TrimEnd().Replace('/', '-')} {dr["sttime"].ToString().TrimEnd()}" } } }, { "endate", new Dictionary<string, object>() { { "data", $"{dr["endate"].ToString().TrimEnd().Replace('/', '-')} {dr["entime"].ToString().TrimEnd()}" } } }, { "success", dr["status"].ToString().TrimEnd() == "0" }, { "failed", dr["status"].ToString().TrimEnd() == "2" } });
            return items;
        }
    }
}