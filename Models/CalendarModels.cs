using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class CalendarClass
    {
        public List<Dictionary<string, object>> GetSearchModels(string clientinfo, string deviceinfo, string dateinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var dateJson = JsonSerializer.Deserialize<Dictionary<string, object>>(dateinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@stdate", DateTime.Parse(dateJson["stdate"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
            dbparams.Add(new dbparam("@endate", DateTime.Parse(dateJson["endate"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.calendarorder @stdate,@endate;", dbparams).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", dr["orderid"].ToString().TrimEnd() } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["shop"].ToString().TrimEnd()) }, { "stdate", new Dictionary<string, object>() { { "data", $"{dr["stdate"].ToString().TrimEnd().Replace('/', '-')} {dr["sttime"].ToString().TrimEnd()}" } } }, { "endate", new Dictionary<string, object>() { { "data", $"{dr["endate"].ToString().TrimEnd().Replace('/', '-')} {dr["entime"].ToString().TrimEnd()}" } } }, { "success", dr["status"].ToString().TrimEnd() == "0" }, { "failed", dr["status"].ToString().TrimEnd() == "2" } });
            }
            return items;
        }

        public List<Dictionary<string, object>> GetCategoryModels(string clientinfo, string deviceinfo, string datainfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, object>>(datainfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@date", DateTime.Parse(dataJson["data"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.calendargroupform @date;", dbparams).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", dr["orderid"].ToString().TrimEnd() } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["shop"].ToString().TrimEnd()) } });
            }
            return items;
        }

        public bool GetDeleteModels(string clientinfo, string deviceinfo, string requiredinfo, string datainfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, object>>(datainfo);            
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            string nowdate = DateTime.Parse(dataJson["data"].ToString().TrimEnd()).ToString("yyyy/MM/dd");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@ondate", nowdate));
            return new database().checkActiveSql("mssql", "eatingstring", "exec eat.deletecalendar @newid,@orderid,@ondate;", dbparams) == "istrue";
        }

        public Dictionary<string, object> GetModifyModels(string clientinfo, string deviceinfo, string requiredinfo, string categoryinfo, string datainfo, string timeinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var categoryJson = JsonSerializer.Deserialize<Dictionary<string, object>>(categoryinfo);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, object>>(datainfo);
            var timeJson = JsonSerializer.Deserialize<Dictionary<string, object>>(timeinfo);
            var noquiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(categoryJson["requireid"].ToString().TrimEnd());
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            string nowdate = DateTime.Parse(dataJson["data"].ToString().TrimEnd()).ToString("yyyy/MM/dd"), nowtime = DateTime.Parse(timeJson["data"].ToString().TrimEnd()).ToString("HH:mm:ss");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@notheid", noquiredJson["orderid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@ondate", nowdate));
            dbparams.Add(new dbparam("@ontime", nowtime));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.modifycalendar @newid,@orderid,@notheid,@ondate,@ontime;", dbparams) != "istrue")
                return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "requireid", noquiredJson }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(categoryJson["shop"].ToString().TrimEnd()) }, { "stdate", new Dictionary<string, object>() { { "data", $"{nowdate.Replace('/', '-')} {nowtime}" } } }, { "endate", new Dictionary<string, object>() { { "data", $"{nowdate.Replace('/', '-')} {nowtime}" } } }, { "success", false }, { "failed", false } };
        }

        public Dictionary<string, object> GetCreateModels(string clientinfo, string deviceinfo, string categoryinfo, string datainfo, string timeinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var categoryJson = JsonSerializer.Deserialize<Dictionary<string, object>>(categoryinfo);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, object>>(datainfo);
            var timeJson = JsonSerializer.Deserialize<Dictionary<string, object>>(timeinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(categoryJson["requireid"].ToString().TrimEnd());
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            string nowdate = DateTime.Parse(dataJson["data"].ToString().TrimEnd()).ToString("yyyy/MM/dd"), nowtime = DateTime.Parse(timeJson["data"].ToString().TrimEnd()).ToString("HH:mm:ss");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@ondate", nowdate));
            dbparams.Add(new dbparam("@ontime", nowtime));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.insertcalendar @newid,@orderid,@ondate,@ontime;", dbparams) != "istrue")
                return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "requireid", requiredJson }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(categoryJson["shop"].ToString().TrimEnd()) }, { "stdate", new Dictionary<string, object>() { { "data", $"{nowdate.Replace('/', '-')} {nowtime}" } } }, { "endate", new Dictionary<string, object>() { { "data", $"{nowdate.Replace('/', '-')} {nowtime}" } } }, { "success", false }, { "failed", false } };
        }
    }
}