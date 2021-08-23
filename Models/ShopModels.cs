using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class ShopClass
    {
        public Dictionary<string, object> GetChooseModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            datetime datetime = new datetime();
            DataTable mainRows = new DataTable();
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec eat.orderestaurant;", new List<dbparam>());
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new Dictionary<string, object>() { { "shop", new Dictionary<string, object>() { { "category", new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", "" } } }, { "shop", new Dictionary<string, object>() { { "name", "" }, { "phone", "" }, { "address", "" } } } } }, { "ordered", false }, { "items", new List<Dictionary<string, object>>() { } } } }, { "time", "" }, { "opened", false }, { "closed", false } };
            }
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
            {
                items.Add(new Dictionary<string, object>() { { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd()) }, { "requireid", new Dictionary<string, object>() { { "orderid", $"{dr["orderid"].ToString().TrimEnd()}{dr["iid"].ToString().TrimEnd()}" } } } });
            }
            string ondate = $"{mainRows.Rows[0]["ondate"].ToString().TrimEnd()} {mainRows.Rows[0]["ontime"].ToString().TrimEnd()}".TrimEnd(), endate = $"{mainRows.Rows[0]["endate"].ToString().TrimEnd()} {mainRows.Rows[0]["entime"].ToString().TrimEnd()}".TrimEnd();
            bool[] business = datetime.checkedBusiness(ondate, endate);
            return new Dictionary<string, object>() { { "shop", new Dictionary<string, object>() { { "category", new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", mainRows.Rows[0]["orderid"].ToString().TrimEnd() } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(mainRows.Rows[0]["shop"].ToString().TrimEnd()) } } }, { "ordered", false }, { "items", items } } }, { "time", datetime.differentimeAbs(business[1] ? endate : ondate) }, { "opened", business[0] }, { "closed", business[1] } };
        }

        public List<Dictionary<string, object>> GetSearchModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            database database = new database();
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in database.checkSelectSql("mssql", "eatingstring", "exec eat.findorderist;", new List<dbparam>()).Rows)
            {
                List<dbparam> dbparams = new List<dbparam>();
                dbparams.Add(new dbparam("@orderid", dr["orderid"].ToString().TrimEnd()));
                List<Dictionary<string, object>> menuitems = new List<Dictionary<string, object>>();
                foreach (DataRow drs in database.checkSelectSql("mssql", "eatingstring", "exec eat.findrestaurant @orderid;", dbparams).Rows)
                {
                    if (!string.IsNullOrWhiteSpace(drs["menu"].ToString().TrimEnd()))
                        menuitems.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", $"{drs["orderid"].ToString().TrimEnd()}{drs["iid"].ToString().TrimEnd()}" } } }, { "menu", JsonSerializer.Deserialize<Dictionary<string, object>>(drs["menu"].ToString().TrimEnd()) } });
                }
                items.Add(new Dictionary<string, object>() { { "category", new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", dr["orderid"].ToString().TrimEnd() } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["shop"].ToString().TrimEnd()) } } }, { "ordered", false }, { "items", menuitems } });
            }
            return items;
        }

        public List<Dictionary<string, object>> GetStatistModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.statistrestaurant;", new List<dbparam>()).Rows)
            {
                var menuJson = JsonSerializer.Deserialize<Dictionary<string, object>>(dr["menu"].ToString().TrimEnd());
                items.Add(new Dictionary<string, object>() { { "statist", new Dictionary<string, object>() { { "key", menuJson["name"].ToString().TrimEnd() }, { "datas", int.Parse(menuJson["quantity"].ToString().TrimEnd()) } } }, { "colors", new int[] { 255, 0, 0, 1 } } });
            }
            return items;
        }

        public List<Dictionary<string, object>> GetCategoryModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.ordershoplist @newid;", dbparams).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", dr["orderid"].ToString().TrimEnd() } } }, { "shop", JsonSerializer.Deserialize<Dictionary<string, object>>(dr["shop"].ToString().TrimEnd()) } });
            }
            return items;
        }

        public Dictionary<string, object> GetClosedModels(string clientinfo, string deviceinfo, string requiredinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            DateTime datetime = DateTime.Now;
            string nowdate = datetime.ToString("yyyy/MM/dd"), nowtime = datetime.ToString("HH:mm:ss");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.closebusiness @newid,@orderid;", dbparams) != "istrue")
                return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "time", new datetime().differentimeAbs($"{nowdate} {nowtime}") } };
        }

        public Dictionary<string, object> GetOpenedModels(string clientinfo, string deviceinfo, string requiredinfo, string timeinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var timeJson = JsonSerializer.Deserialize<Dictionary<string, object>>(timeinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            datetime datetime = new datetime();
            string nowdate = DateTime.Now.ToString("yyyy/MM/dd"), nowtime = DateTime.Parse(timeJson["data"].ToString().TrimEnd()).ToString("HH:mm:ss");
            int addate = datetime.differentimeInt($"{nowdate} {nowtime}");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@addate", addate > 0 ? 1 : 0));
            dbparams.Add(new dbparam("@ontime", nowtime));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.openbusiness @newid,@orderid,@addate,@ontime;", dbparams) != "istrue")
                return new Dictionary<string, object>() { };
            return new ShopClass().GetChooseModels(clientinfo, deviceinfo, cuurip);
        }

        public Dictionary<string, object> GetWaitedModels(string clientinfo, string deviceinfo, string requiredinfo, string timeinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var timeJson = JsonSerializer.Deserialize<Dictionary<string, object>>(timeinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            datetime datetime = new datetime();
            string nowdate = DateTime.Now.ToString("yyyy/MM/dd"), nowtime = DateTime.Parse(timeJson["data"].ToString().TrimEnd()).ToString("HH:mm:ss");
            int addate = datetime.differentimeInt($"{nowdate} {nowtime}");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@addate", addate > 0 ? 1 : 0));
            dbparams.Add(new dbparam("@ontime", nowtime));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.waitbusiness @newid,@orderid,@addate,@ontime;", dbparams) != "istrue")
                return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "time", datetime.differentimeAbs($"{nowdate} {nowtime}") } };
        }

        public Dictionary<string, object> GetRestartModels(string clientinfo, string deviceinfo, string requiredinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            DateTime datetime = DateTime.Now;
            string nowdate = datetime.ToString("yyyy/MM/dd"), nowtime = datetime.ToString("HH:mm:ss");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.restartbusiness @newid,@orderid;", dbparams) != "istrue")
                return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "time", new datetime().differentimeAbs($"{nowdate} {nowtime}") } };
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
            dbparams.Add(new dbparam("@iid", iid));
            dbparams.Add(new dbparam("@orderid", orderid));
            return new database().checkActiveSql("mssql", "eatingstring", "exec eat.deletemenuform @newid,@iid,@orderid;", dbparams) == "istrue";
        }

        public Dictionary<string, object> GetModifyModels(string clientinfo, string deviceinfo, string requiredinfo, string iteminfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var itemJson = JsonSerializer.Deserialize<Dictionary<string, object>>(iteminfo);
            if (!int.TryParse(itemJson["value"].ToString().TrimEnd(), out int store)) return new Dictionary<string, object>() { };
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            int orderLen = requiredJson["orderid"].ToString().TrimEnd().Length - 1;
            string iid = requiredJson["orderid"].ToString().TrimEnd().Substring(orderLen),
                orderid = requiredJson["orderid"].ToString().TrimEnd().Substring(0, orderLen);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@iid", iid));
            dbparams.Add(new dbparam("@orderid", orderid));
            dbparams.Add(new dbparam("@menu", JsonSerializer.Serialize(new Dictionary<string, object>() { { "name", itemJson["key"].ToString().TrimEnd() }, { "price", itemJson["value"].ToString().TrimEnd() } })));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.updatemenuform @newid,@iid,@orderid,@menu;", dbparams) != "istrue")
                return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "menu", new Dictionary<string, object>() { { "name", itemJson["key"].ToString().TrimEnd() }, { "price", itemJson["value"].ToString().TrimEnd() }, { "quantity", "0" } } }, { "requireid", new Dictionary<string, object>() { { "orderid", requiredJson["orderid"].ToString().TrimEnd() } } } };
        }

        public Dictionary<string, object> GetInsertModels(string clientinfo, string deviceinfo, string requiredinfo, string iteminfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var requiredJson = JsonSerializer.Deserialize<Dictionary<string, object>>(requiredinfo);
            var itemJson = JsonSerializer.Deserialize<Dictionary<string, object>>(iteminfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            if (!int.TryParse(itemJson["value"].ToString().TrimEnd(), out int store)) return new Dictionary<string, object>() { };
            string newid = new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}");
            database database = new database();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@orderid", requiredJson["orderid"].ToString().TrimEnd()));
            string iid = database.checkSelectSql("mssql", "eatingstring", "exec eat.finalordermenu @orderid;", dbparams).Rows[0]["iid"].ToString().TrimEnd();
            dbparams.Add(new dbparam("@iid", iid));
            dbparams.Add(new dbparam("@menu", JsonSerializer.Serialize(new Dictionary<string, object>() { { "name", itemJson["key"].ToString().TrimEnd() }, { "price", itemJson["value"].ToString().TrimEnd() } })));
            dbparams.Add(new dbparam("@newid", newid));
            if (database.checkActiveSql("mssql", "eatingstring", "exec eat.insertmenuform @iid,@orderid,@menu,@newid;", dbparams) != "istrue")
                return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "menu", new Dictionary<string, object>() { { "name", itemJson["key"].ToString().TrimEnd() }, { "price", itemJson["value"].ToString().TrimEnd() }, { "quantity", "0" } } }, { "requireid", new Dictionary<string, object>() { { "orderid", $"{requiredJson["orderid"].ToString().TrimEnd()}{iid}" } } } };
        }

        public Dictionary<string, object> GetCreateModels(string clientinfo, string deviceinfo, string shopinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var shopJson = JsonSerializer.Deserialize<Dictionary<string, object>>(shopinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            sha256 sha256 = new sha256();
            string orderid = sha256.new256("mssql", "eatingstring"), newid = sha256.encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@orderid", orderid));
            dbparams.Add(new dbparam("@shop", JsonSerializer.Serialize(new Dictionary<string, object>() { { "name", shopJson["name"].ToString().TrimEnd() }, { "phone", shopJson["phone"].ToString().TrimEnd() }, { "address", shopJson["address"].ToString().TrimEnd() } })));
            dbparams.Add(new dbparam("@newid", newid));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.insertorderform @orderid,@shop,@newid;", dbparams) != "istrue") return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "requireid", new Dictionary<string, object>() { { "orderid", orderid } } }, { "shop", shopJson }, { "ordered", false }, { "items", new List<Dictionary<string, object>>() } };
        }
    }
}