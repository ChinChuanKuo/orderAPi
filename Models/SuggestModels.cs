using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class SuggestClass
    {
        public List<Dictionary<string, object>> GetSearchModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "eat.searchsuggest @newid;", dbparams).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", dr["orderid"].ToString().TrimEnd() } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["shop"].ToString().TrimEnd()) }, { "date", new Dictionary<string, object>() { { "data", dr["indate"].ToString().TrimEnd() } } }, { "time", new Dictionary<string, object>() { { "data", dr["intime"].ToString().TrimEnd() } } } });
            }
            return items;
        }

        public bool GetInsertModels(string clientinfo, string deviceinfo, string shopinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var shopJson = JsonSerializer.Deserialize<Dictionary<string, object>>(shopinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@shop", JsonSerializer.Serialize(new Dictionary<string, object>() { { "name", shopJson["name"].ToString().TrimEnd() }, { "phone", shopJson["phone"].ToString().TrimEnd() }, { "address", shopJson["address"].ToString().TrimEnd() } })));
            return new database().checkActiveSql("mssql", "eatingstring", "exec eat.insertsuggest @newid,@shop;", dbparams) == "istrue";
        }

        public bool GetDeleteModels(string clientinfo, string deviceinfo, string requiredinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            return new database().checkActiveSql("mssql", "eatingstring", "exec eat.deletesuggest @orderid,@newid;", dbparams) == "istrue";
        }

        public Dictionary<string, object> GetCreateModels(string clientinfo, string deviceinfo, string requiredinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            sha256 sha256 = new sha256();
            string orderid = sha256.new256("mssql", "eatingstring"), newid = sha256.encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}");
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@newid", newid));
            mainRows = database.checkSelectSql("mssql", "eatingstring", "exec eat.findsuggest @orderid,@newid;", dbparams);
            if (mainRows.Rows.Count == 0) return new Dictionary<string, object>() { };
            dbparams.Add(new dbparam("@neworid", orderid));
            dbparams.Add(new dbparam("@shop", mainRows.Rows[0]["shop"].ToString().TrimEnd()));
            if (database.checkActiveSql("mssql", "eatingstring", "exec eat.createsuggest @orderid,@neworid,@shop,@newid;", dbparams) != "istrue") return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", orderid } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(mainRows.Rows[0]["shop"].ToString().TrimEnd()) }, { "ordered", false }, { "items", new List<Dictionary<string, object>>() } };
        }
    }
}