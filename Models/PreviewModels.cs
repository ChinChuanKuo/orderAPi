using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class PreviewClass
    {
        public Dictionary<string, object> GetSearchModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<Dictionary<string, object>> item = new List<Dictionary<string, object>>()
            {
                { new Dictionary<string, object>() { { "data", "姓名" } } },
                { new Dictionary<string, object>() { { "data", "飯盒名稱" } } },
                { new Dictionary<string, object>() { { "data", "數量" } } },
                { new Dictionary<string, object>() { { "data", "金額" } } }
            };
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.previewcheckout @newid;", dbparams).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", $"{dr["orderid"].ToString().TrimEnd()}{dr["iid"].ToString().TrimEnd()}" } } }, { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd()) }, { "client", new Dictionary<string, object>() { { "clientid", dr["clientid"].ToString().TrimEnd() }, { "accesstoken", dr["accesstoken"].ToString().TrimEnd() }, { "email", dr["signdata"].ToString().TrimEnd() } } } });
            }
            return new Dictionary<string, object>() { { "item", item }, { "items", items } };
        }

        public Dictionary<string, object> GetTotalModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<Dictionary<string, object>> item = new List<Dictionary<string, object>>()
            {
                { new Dictionary<string, object>() { { "data", "飯盒名稱" } } },
                { new Dictionary<string, object>() { { "data", "總數量 x 金額" } } },
                { new Dictionary<string, object>() { { "data", "總金額" } } }
            };
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.previewtotal @newid;", dbparams).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", $"{dr["orderid"].ToString().TrimEnd()}{dr["iid"].ToString().TrimEnd()}" } } }, { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd()) } });
            }
            return new Dictionary<string, object>() { { "item", item }, { "items", items } };
        }
    }
}