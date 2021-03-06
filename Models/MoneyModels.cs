using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class MoneyClass
    {
        public Dictionary<string, object> GetBalenceModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            return new Dictionary<string, object>() { { "item", new Dictionary<string, object>() { { "data", new database().checkSelectSql("mssql", "eatingstring", "exec eat.storewalletform @newid;", dbparams).Rows[0]["itemcount"].ToString().TrimEnd() } } } };
        }

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
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.storestoreform @newid,@stdate,@endate;", dbparams).Rows)
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", dr["storeid"].ToString().TrimEnd() } } }, { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd()) }, { "money", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["store"].ToString().TrimEnd()) }, { "stdate", new Dictionary<string, object>() { { "data", $"{dr["indate"].ToString().TrimEnd().Replace('/', '-')} {dr["intime"].ToString().TrimEnd()}" } } }, { "endate", new Dictionary<string, object>() { { "data", $"{dr["indate"].ToString().TrimEnd().Replace('/', '-')} {dr["intime"].ToString().TrimEnd()}" } } }, { "success", dr["status"].ToString().TrimEnd() == "1" }, { "failed", dr["status"].ToString().TrimEnd() == "2" } });
            return items;
        }

        public List<Dictionary<string, object>> GetStatistModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            string newid = new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}");
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            for (int i = 4; i >= 0; i--)
            {
                string yearMonth = DateTime.Now.AddMonths(-i).ToString("yyyy/MM");
                List<dbparam> dbparams = new List<dbparam>();
                dbparams.Add(new dbparam("@newid", newid));
                dbparams.Add(new dbparam("@year", yearMonth));
                items.Add(new Dictionary<string, object>() { { "statist", new Dictionary<string, object>() { { "key", yearMonth }, { "datas", int.Parse(new database().checkSelectSql("mssql", "eatingstring", "exec eat.statiststoreform @newid,@year;", dbparams).Rows[0]["itemcount"].ToString().TrimEnd()) } } }, { "colors", new int[] { 255, 0, 0, 1 } } });
            }
            return items;
        }

        public List<Dictionary<string, object>> GetAnalysisModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.analysistore @newid;", dbparams).Rows)
                items.Add(new Dictionary<string, object>() { { "statist", new Dictionary<string, object>() { { "key", dr["title"].ToString().TrimEnd() }, { "datas", int.Parse(dr["itemcount"].ToString().TrimEnd()) } } }, { "colors", new int[] { 255, 0, 0, 1 } } });
            return items;
        }
    }
}