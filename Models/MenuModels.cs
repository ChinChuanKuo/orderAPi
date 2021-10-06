using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class MenuClass
    {
        public Dictionary<string, object> GetOfficeModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@month", DateTime.Now.ToString("MM")));
            Dictionary<string, object> item = new Dictionary<string, object>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.officemenuform @newid,@month;", dbparams).Rows)
                if (item.Count == 0)
                    item.Add("data", dr["username"].ToString().TrimEnd());
                else
                    item["data"] += $" / {dr["username"].ToString().TrimEnd()}";
            return item;
        }

        public Dictionary<string, object> GetSearchModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec eat.searchmenuform @newid;", dbparams);
            if (mainRows.Rows.Count == 0)
                return new Dictionary<string, object>() { };
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", $"{dr["orderid"].ToString().TrimEnd()}{dr["iid"].ToString().TrimEnd()}" } } }, { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd()) }, { "ordered", dr["ordered"].ToString().TrimEnd() == "1" }, { "action", new Dictionary<string, object>() { { "inserted", false }, { "modified", false }, { "deleted", dr["ordered"].ToString().TrimEnd() == "1" } } } });
            string ondate = $"{mainRows.Rows[0]["ondate"].ToString().TrimEnd()} {mainRows.Rows[0]["ontime"].ToString().TrimEnd()}".TrimEnd(), endate = $"{mainRows.Rows[0]["endate"].ToString().TrimEnd()} {mainRows.Rows[0]["entime"].ToString().TrimEnd()}".TrimEnd();
            bool[] business = new datetime().checkedBusiness(ondate, endate);
            return new Dictionary<string, object>() { { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(mainRows.Rows[0]["shop"].ToString().TrimEnd()) }, { "items", items }, { "opened", business[0] }, { "closed", business[1] } };
        }

        public List<Dictionary<string, object>> GetOrderModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.ordermenuform @newid;", dbparams).Rows)
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", $"{dr["orderid"].ToString().TrimEnd()}{dr["iid"].ToString().TrimEnd()}" } } }, { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd()) }, { "ordered", dr["ordered"].ToString().TrimEnd() == "1" }, { "action", new Dictionary<string, object>() { { "inserted", false }, { "modified", false }, { "deleted", dr["ordered"].ToString().TrimEnd() == "1" } } } });
            return items;
        }

        public Dictionary<string, object> GetInsertModels(string clientinfo, string deviceinfo, string menuinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var menuJson = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(menuinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            string newid = new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}");
            database database = new database();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", newid));
            if (database.checkSelectSql("mssql", "eatingstring", "exec eat.searchmenuform @newid;", dbparams).Rows[0]["endate"].ToString().TrimEnd() != "")
                return new Dictionary<string, object>() { { "status", "closed" } };
            if (!countOrder(menuJson, newid))
                return new Dictionary<string, object>() { { "status", "enough" } };
            foreach (var menu in menuJson)
            {
                var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(menu["requireid"].ToString().TrimEnd());
                var actionJson = JsonSerializer.Deserialize<Dictionary<string, object>>(menu["action"].ToString().TrimEnd());
                string[] substr256 = new sha256().substr256(requiredJson["orderid"].ToString().TrimEnd(), 36);
                dbparams = new List<dbparam>();
                dbparams.Add(new dbparam("@newid", newid));
                dbparams.Add(new dbparam("@iid", substr256[0]));
                dbparams.Add(new dbparam("@orderid", substr256[1]));
                dbparams.Add(new dbparam("@menu", menu["menu"].ToString().TrimEnd()));
                switch (bool.Parse(actionJson["inserted"].ToString().TrimEnd()))
                {
                    case true:
                        if (database.checkActiveSql("mssql", "eatingstring", "exec eat.insertcheckout @newid,@iid,@orderid,@menu;", dbparams) != "istrue")
                            return new Dictionary<string, object>() { };
                        break;
                    default:
                        if (bool.Parse(actionJson["modified"].ToString().TrimEnd()))
                            if (database.checkActiveSql("mssql", "eatingstring", "exec eat.updatecheckout @newid,@iid,@orderid,@menu;", dbparams) != "istrue")
                                return new Dictionary<string, object>() { };
                        break;
                }
                Thread.Sleep(1000);
            }
            return new Dictionary<string, object>() { { "status", "istrue" } };
        }

        public bool countOrder(List<Dictionary<string, object>> menuJson, string newid)
        {
            if (string.IsNullOrWhiteSpace(newid)) return false;
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", newid));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec eat.checkwallet @newid;", dbparams);
            if (mainRows.Rows.Count == 0) return false;
            int count = 0;
            foreach (var menu in menuJson)
            {
                var menuitem = JsonSerializer.Deserialize<Dictionary<string, object>>(menu["menu"].ToString().TrimEnd());
                count += int.Parse(menuitem["price"].ToString().TrimEnd()) * int.Parse(menuitem["quantity"].ToString().TrimEnd());
            }
            return int.Parse(mainRows.Rows[0]["money"].ToString().TrimEnd()) >= count;
        }

        public Dictionary<string, object> GetDeleteModels(string clientinfo, string deviceinfo, string requiredinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            string newid = new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}");
            /*int orderLen = requiredJson["orderid"].ToString().TrimEnd().Length - 1;
            string iid = requiredJson["orderid"].ToString().TrimEnd().Substring(orderLen),
                orderid = requiredJson["orderid"].ToString().TrimEnd().Substring(0, orderLen);*/
            string[] substr256 = new sha256().substr256(requiredJson["orderid"].ToString().TrimEnd(), 36);
            database database = new database();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", newid));
            if (database.checkSelectSql("mssql", "eatingstring", "exec eat.searchmenuform @newid;", dbparams).Rows[0]["endate"].ToString().TrimEnd() != "")
                return new Dictionary<string, object>() { { "status", "closed" } };
            dbparams.Add(new dbparam("@iid", substr256[0]));
            dbparams.Add(new dbparam("@orderid", substr256[1]));
            if (database.checkActiveSql("mssql", "eatingstring", "exec eat.deletecheckout @newid,@iid,@orderid;", dbparams) == "istrue")
                return new Dictionary<string, object>() { { "status", "istrue" } };
            return new Dictionary<string, object>() { };
        }
    }
}