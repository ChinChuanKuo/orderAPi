using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class MenuClass
    {
        public Dictionary<string, object> GetSearchModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec eat.ordermenuform @newid;", dbparams);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new Dictionary<string, object>() { };
            }
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
            {
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", $"{dr["orderid"].ToString().TrimEnd()}{dr["iid"].ToString().TrimEnd()}" } } }, { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd()) }, { "ordered", dr["ordered"].ToString().TrimEnd() == "1" }, { "action", new Dictionary<string, object>() { { "inserted", false }, { "modified", false }, { "deleted", dr["ordered"].ToString().TrimEnd() == "1" } } } });
            }
            return new Dictionary<string, object>() { { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(mainRows.Rows[0]["shop"].ToString().TrimEnd()) }, { "items", items }, { "closed", !string.IsNullOrWhiteSpace(mainRows.Rows[0]["endate"].ToString().TrimEnd()) } };
        }

        public bool GetInsertModels(string clientinfo, string deviceinfo, string menuinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var menuJson = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(menuinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            string newid = new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}");
            foreach (var menu in menuJson)
            {
                var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(menu["requireid"].ToString().TrimEnd());
                var actionJson = JsonSerializer.Deserialize<Dictionary<string, object>>(menu["action"].ToString().TrimEnd());
                int orderLen = requiredJson["orderid"].ToString().TrimEnd().Length - 1;
                string iid = requiredJson["orderid"].ToString().TrimEnd().Substring(orderLen),
                    orderid = requiredJson["orderid"].ToString().TrimEnd().Substring(0, orderLen);
                List<dbparam> dbparams = new List<dbparam>();
                dbparams.Add(new dbparam("@newid", newid));
                dbparams.Add(new dbparam("@iid", iid));
                dbparams.Add(new dbparam("@orderid", orderid));
                dbparams.Add(new dbparam("@menu", menu["menu"].ToString().TrimEnd()));
                switch (bool.Parse(actionJson["inserted"].ToString().TrimEnd()))
                {
                    case true:
                        if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.insertcheckout @newid,@iid,@orderid,@menu;", dbparams) != "istrue")
                            return false;
                        break;
                    default:
                        switch (bool.Parse(actionJson["modified"].ToString().TrimEnd()))
                        {
                            case true:
                                if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.updatecheckout @newid,@iid,@orderid,@menu;", dbparams) != "istrue")
                                    return false;
                                break;
                        }
                        break;
                }
            }
            return true;
        }

        public bool GetDeleteModels(string clientinfo, string deviceinfo, string requiredinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            int orderLen = requiredJson["orderid"].ToString().TrimEnd().Length - 1;
            string iid = requiredJson["orderid"].ToString().TrimEnd().Substring(orderLen),
                orderid = requiredJson["orderid"].ToString().TrimEnd().Substring(0, orderLen);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@orderid", orderid));
            dbparams.Add(new dbparam("@iid", iid));
            return new database().checkActiveSql("mssql", "eatingstring", "exec eat.deletecheckout @newid,@orderid,@iid;", dbparams) == "istrue";
        }
    }
}