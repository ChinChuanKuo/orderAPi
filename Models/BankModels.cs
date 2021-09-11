using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class BankClass
    {
        public Dictionary<string, object> GetBalenceModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            return new Dictionary<string, object>() { { "item", new Dictionary<string, object>() { { "data", new database().checkSelectSql("mssql", "eatingstring", "exec eat.bankwalletform;", new List<dbparam>()).Rows[0]["itemcount"].ToString().TrimEnd() } } } };
        }

        public List<Dictionary<string, object>> GetSearchModels(string clientinfo, string deviceinfo, string dateinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var dateJson = JsonSerializer.Deserialize<Dictionary<string, object>>(dateinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            database database = new database();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@stdate", DateTime.Parse(dateJson["stdate"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
            dbparams.Add(new dbparam("@endate", DateTime.Parse(dateJson["endate"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in database.checkSelectSql("mssql", "eatingstring", "exec eat.bankstoreform @stdate,@endate;", dbparams).Rows)
            {
                var money = JsonSerializer.Deserialize<Dictionary<string, object>>(dr["store"].ToString().TrimEnd());
                dbparams.Clear();
                dbparams.Add(new dbparam("@clientid", dr["clientid"].ToString().TrimEnd()));
                dbparams.Add(new dbparam("@accesstoken", dr["accesstoken"].ToString().TrimEnd()));
                dbparams.Add(new dbparam("@category", money["category"].ToString().TrimEnd()));
                dbparams.Add(new dbparam("@stdate", DateTime.Parse(dateJson["stdate"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
                dbparams.Add(new dbparam("@endate", DateTime.Parse(dateJson["endate"].ToString().TrimEnd()).ToString("yyyy/MM/dd")));
                foreach (DataRow row in database.checkSelectSql("mssql", "eatingstring", "exec eat.bankinfoform @clientid,@accesstoken,@category,@stdate,@endate;", dbparams).Rows)
                {
                    items.Add(new Dictionary<string, object>() { { "money", money }, { "requireid", new Dictionary<string, object>() { { "orderid", row["storeid"].ToString().TrimEnd() } } }, { "client", new Dictionary<string, object>() { { "clientid", dr["clientid"].ToString().TrimEnd() }, { "accesstoken", dr["accesstoken"].ToString().TrimEnd() }, { "email", dr["email"].ToString().TrimEnd() } } }, { "stdate", new Dictionary<string, object>() { { "data", $"{row["indate"].ToString().TrimEnd().Replace('/', '-')} {row["intime"].ToString().TrimEnd()}" } } }, { "endate", new Dictionary<string, object>() { { "data", $"{row["indate"].ToString().TrimEnd().Replace('/', '-')} {row["intime"].ToString().TrimEnd()}" } } }, { "success", dr["status"].ToString().TrimEnd() == "1" }, { "failed", dr["status"].ToString().TrimEnd() == "2" } });
                }
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
                items.Add(new Dictionary<string, object>() { { "statist", new Dictionary<string, object>() { { "key", yearMonth }, { "datas", int.Parse(new database().checkSelectSql("mssql", "eatingstring", "exec eat.statistbankform @year;", dbparams).Rows[0]["itemcount"].ToString().TrimEnd()) } } }, { "colors", new int[] { 255, 0, 0, 1 } } });
            }
            return items;
        }

        public Dictionary<string, object> GetCheckModels(string clientinfo, string deviceinfo, string infotext, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            var notherJson = new UserinfoClass().checkAccess(infotext);
            var nondomJson = new UserinfoClass().checkRandom(new Dictionary<string, object>() { { "clientid", notherJson["clientid"].ToString().TrimEnd() }, { "accesstoken", notherJson["accesstoken"].ToString().TrimEnd() } });
            string notid = new sha256().encry256($"{notherJson["clientid"].ToString().TrimEnd()}{nondomJson["random"].ToString().TrimEnd()}{notherJson["accesstoken"].ToString().TrimEnd()}");
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", notid));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec eat.findwallet @newid;", dbparams);
            if (mainRows.Rows.Count == 0) return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "item", new Dictionary<string, object>() { { "data", mainRows.Rows[0]["money"].ToString().TrimEnd() } } }, { "client", notherJson } };
        }

        public Dictionary<string, object> GetInsertModels(string clientinfo, string deviceinfo, string notherinfo, string storetext, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var notherJson = JsonSerializer.Deserialize<Dictionary<string, object>>(notherinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            var nondomJson = new UserinfoClass().checkRandom(new Dictionary<string, object>() { { "clientid", notherJson["clientid"].ToString().TrimEnd() }, { "accesstoken", notherJson["accesstoken"].ToString().TrimEnd() } });
            if (!int.TryParse(storetext, out int store)) return new Dictionary<string, object>() { };
            DateTime nowdate = DateTime.Now;
            sha256 sha256 = new sha256();
            string category = int.Parse(storetext) >= 0 ? "Comfirm Stored" : "Comfirm Pickup", storeid = sha256.new256("mssql", "eatingstring"),
            newid = sha256.encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}"),
            notid = sha256.encry256($"{notherJson["clientid"].ToString().TrimEnd()}{nondomJson["random"].ToString().TrimEnd()}{notherJson["accesstoken"].ToString().TrimEnd()}");
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", notid));
            dbparams.Add(new dbparam("@storeid", storeid));
            dbparams.Add(new dbparam("@store", JsonSerializer.Serialize(new Dictionary<string, object>() { { "store", storetext.Trim() }, { "category", category } })));
            dbparams.Add(new dbparam("@inoper", newid));
            if (new database().checkActiveSql("mssql", "eatingstring", "exec eat.insertbankform @newid,@storeid,@store,@inoper;", dbparams) != "istrue") return new Dictionary<string, object>() { };
            if (!new NotificationsClass().noticeModels(notid, new Dictionary<string, object>() { { "imageUrl", "https://images.unsplash.com/photo-1525253086316-d0c936c814f8" }, { "message", $"{category}：{storetext.Trim()}" } }, newid)) return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "money", new Dictionary<string, object>() { { "store", storetext.Trim() }, { "category", category }, } }, { "requireid", new Dictionary<string, object>() { { "orderid", storeid } } }, { "client", notherJson }, { "stdate", new Dictionary<string, object>() { { "data", $"{nowdate.ToString("yyyy/MM/dd").Replace('/', '-')} {nowdate.ToString("HH:mm:ss")}" } } }, { "endate", new Dictionary<string, object>() { { "data", $"{nowdate.ToString("yyyy/MM/dd").Replace('/', '-')} {nowdate.ToString("HH:mm:ss")}" } } }, { "success", true }, { "failed", false } };
        }
    }
}