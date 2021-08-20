using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class OrderClass
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
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.ordercheckout @stdate,@endate;", dbparams).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", $"{dr["orderid"].ToString().TrimEnd()}{dr["iid"].ToString().TrimEnd()}" } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["shop"].ToString().TrimEnd()) }, { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd()) }, { "stdate", new Dictionary<string, object>() { { "data", $"{dr["stdate"].ToString().TrimEnd().Replace('/', '-')} {dr["sttime"].ToString().TrimEnd()}" } } }, { "endate", new Dictionary<string, object>() { { "data", $"{dr["endate"].ToString().TrimEnd().Replace('/', '-')} {dr["entime"].ToString().TrimEnd()}" } } }, { "success", dr["status"].ToString().TrimEnd() == "0" }, { "failed", dr["status"].ToString().TrimEnd() == "2" } });
            }
            return items;
        }

        public List<Dictionary<string, object>> GetStatistModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
           for (int i = 4; i >= 0; i--)
            {
                string yearMonth = DateTime.Now.AddMonths(-i).ToString("yyyy/MM");
                List<dbparam> dbparams = new List<dbparam>();
                dbparams.Add(new dbparam("@year", yearMonth));
                items.Add(new Dictionary<string, object>() { { "statist", new Dictionary<string, object>() { { "key", yearMonth }, { "datas", int.Parse(new database().checkSelectSql("mssql", "eatingstring", "exec eat.statistordercheckout @year;", dbparams).Rows[0]["itemcount"].ToString().TrimEnd()) } } }, { "colors", new int[] { 255, 0, 0, 1 } } });
            }
            return items;
        }

        public List<Dictionary<string, object>> GetAnalysisModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.analysisordercheckout;", new List<dbparam>()).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "statist", new Dictionary<string, object>() { { "key", dr["title"].ToString().TrimEnd() }, { "datas", int.Parse(dr["itemcount"].ToString().TrimEnd()) } } }, { "colors", new int[] { 255, 0, 0, 1 } } });
            }
            return items;
        }
    }
}