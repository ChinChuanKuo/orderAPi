using System.Collections.Generic;
using System.Text.Json;
using System.Data;
using orderAPi.App_Code;
using System;

namespace orderAPi.Models
{
    public class OfficeClass
    {
        public Dictionary<string, object> GetSigninModels(string signinfo, string deviceinfo, string cuurip)
        {
            var signJson = JsonSerializer.Deserialize<Dictionary<string, object>>(signinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            sha256 sha256 = new sha256();
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@userid", signJson["email"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@password", sha256.encry256(signJson["value"].ToString().TrimEnd())));
            dbparams.Add(new dbparam("@month", DateTime.Now.ToString("MM")));
            mainRows = database.checkSelectSql("mssql", "eatingstring", "exec web.checkofficeber @userid,@password,@month;", dbparams);
            if (mainRows.Rows.Count == 0) return new Dictionary<string, object>() { };
            if (mainRows.Rows[0]["isused"].ToString().TrimEnd() == "1")
            {
                dbparams.Clear();
                DataTable subRows = new DataTable();
                dbparams.Add(new dbparam("@newid", sha256.encry256(mainRows.Rows[0]["clientid"].ToString().TrimEnd() + mainRows.Rows[0]["random"].ToString().TrimEnd() + mainRows.Rows[0]["accesstoken"].ToString().TrimEnd())));
                dbparams.Add(new dbparam("@externip", cuurip));
                subRows = database.checkSelectSql("mssql", "eatingstring", "exec web.checksitelog @newid,@externip;", dbparams);
                switch (subRows.Rows.Count)
                {
                    case 0:
                        dbparams.Add(new dbparam("@device", deviceinfo));
                        if (database.checkActiveSql("mssql", "eatingstring", "exec web.insertsitelog @newid,@externip,@device;", dbparams) != "istrue")
                            return new Dictionary<string, object>() { };
                        return new Dictionary<string, object>() { { "clientid", mainRows.Rows[0]["clientid"].ToString().TrimEnd() }, { "accesstoken", mainRows.Rows[0]["accesstoken"].ToString().TrimEnd() } };
                }
                if (subRows.Rows[0]["islogin"].ToString().TrimEnd() == "1")
                {
                    dbparams.Add(new dbparam("@device", deviceinfo));
                    if (database.checkActiveSql("mssql", "eatingstring", "exec web.updatesitelog @newid,@externip,@device;", dbparams) != "istrue")
                        return new Dictionary<string, object>() { };
                    return new Dictionary<string, object>() { { "clientid", mainRows.Rows[0]["clientid"].ToString().TrimEnd() }, { "accesstoken", mainRows.Rows[0]["accesstoken"].ToString().TrimEnd() } };
                }
            }
            return new Dictionary<string, object>() { };
        }

        public List<Dictionary<string, object>> GetSearchModels(string clientinfo, string deviceinfo, string datainfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, object>>(datainfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@month", DateTime.Parse(dataJson["data"].ToString().TrimEnd()).ToString("MM")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.searchoffice @newid,@month;", dbparams).Rows)
            {
                items.Add(new Dictionary<string, object>() { { "item", new Dictionary<string, object>() { { "data", dr["userid"].ToString().TrimEnd() } } }, { "client", new Dictionary<string, object>() { { "clientid", dr["clientid"].ToString().TrimEnd() }, { "accesstoken", dr["accesstoken"].ToString().TrimEnd() }, { "email", dr["username"].ToString().TrimEnd() } } }, { "action", new Dictionary<string, object>() { { "inserted", false }, { "modified", false }, { "deleted", true } } } });
            }
            return items;
        }

        public Dictionary<string, object> GetCheckModels(string clientinfo, string deviceinfo, string datainfo, string infotext, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, object>>(datainfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            var notherJson = new UserinfoClass().checkAccess(infotext);
            var nondomJson = new UserinfoClass().checkRandom(new Dictionary<string, object>() { { "clientid", notherJson["clientid"].ToString().TrimEnd() }, { "accesstoken", notherJson["accesstoken"].ToString().TrimEnd() } });
            string notid = new sha256().encry256($"{notherJson["clientid"].ToString().TrimEnd()}{nondomJson["random"].ToString().TrimEnd()}{notherJson["accesstoken"].ToString().TrimEnd()}");
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", notid));
            dbparams.Add(new dbparam("@month", DateTime.Parse(dataJson["data"].ToString().TrimEnd()).ToString("MM")));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec eat.findoffice @newid,@month;", dbparams);
            if (mainRows.Rows.Count == 0)
                return new Dictionary<string, object>() { { "item", new Dictionary<string, object>() { { "data", "" } } }, { "client", new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" }, { "email", "" } } }, { "action", new Dictionary<string, object>() { { "inserted", false }, { "modified", false }, { "deleted", false } } } };
            return new Dictionary<string, object>() { { "item", new Dictionary<string, object>() { { "data", mainRows.Rows[0]["userid"].ToString().TrimEnd() } } }, { "client", notherJson }, { "action", new Dictionary<string, object>() { { "inserted", true }, { "modified", true }, { "deleted", false } } } };
        }

        public bool GetInsertModels(string clientinfo, string deviceinfo, string officeinfo, string datainfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var officeJson = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(officeinfo);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, object>>(datainfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            database database = new database();
            string newid = new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}"),
            month = DateTime.Parse(dataJson["data"].ToString().TrimEnd()).ToString("MM");
            foreach (var office in officeJson)
            {
                var itemJson = JsonSerializer.Deserialize<Dictionary<string, object>>(office["item"].ToString().TrimEnd());
                var itemsJson = JsonSerializer.Deserialize<Dictionary<string, object>>(office["client"].ToString().TrimEnd());
                var nondomJson = new UserinfoClass().checkRandom(new Dictionary<string, object>() { { "clientid", itemsJson["clientid"].ToString().TrimEnd() }, { "accesstoken", itemsJson["accesstoken"].ToString().TrimEnd() } });
                string notid = new sha256().encry256($"{itemsJson["clientid"].ToString().TrimEnd()}{nondomJson["random"].ToString().TrimEnd()}{itemsJson["accesstoken"].ToString().TrimEnd()}");
                List<dbparam> dbparams = new List<dbparam>();
                dbparams.Add(new dbparam("@newid", newid));
                dbparams.Add(new dbparam("@notid", notid));
                dbparams.Add(new dbparam("@month", month));
                if (database.checkActiveSql("mssql", "eatingstring", "exec eat.insertoffice @newid,@notid,@month;", dbparams) != "istrue")
                    return false;
            }
            return true;
        }

        public bool GetDeleteModels(string clientinfo, string deviceinfo, string notherinfo, string datainfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var notherJson = JsonSerializer.Deserialize<Dictionary<string, object>>(notherinfo);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, object>>(datainfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            var nondomJson = new UserinfoClass().checkRandom(new Dictionary<string, object>() { { "clientid", notherJson["clientid"].ToString().TrimEnd() }, { "accesstoken", notherJson["accesstoken"].ToString().TrimEnd() } });
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@notid", new sha256().encry256($"{notherJson["clientid"].ToString().TrimEnd()}{nondomJson["random"].ToString().TrimEnd()}{notherJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@month", DateTime.Parse(dataJson["data"].ToString().TrimEnd()).ToString("MM")));
            return new database().checkActiveSql("mssql", "eatingstring", "exec eat.deleteoffice @newid,@notid,@month;", dbparams) == "istrue";
        }
    }
}