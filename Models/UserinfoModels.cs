using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class UserinfoClass
    {
        public userModels GetSearchModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@externip", cuurip));
            if (new database().checkSelectSql("mssql", "eatingstring", "exec web.checkuserinfo @newid,@externip;", dbparams).Rows.Count == 0)
                return new userModels() { client = new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" } } };
            return new userModels() { client = new Dictionary<string, object>() { { "clientid", clientJson["clientid"].ToString().TrimEnd() }, { "accesstoken", clientJson["accesstoken"].ToString().TrimEnd() }, { "email", "" } }, name = randomJson["username"].ToString().TrimEnd(), imageUrl = "https://images.unsplash.com/photo-1525253086316-d0c936c814f8" };
        }

        public userModels GetOfficeModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@externip", cuurip));
            dbparams.Add(new dbparam("@month", DateTime.Now.ToString("MM")));
            if (new database().checkSelectSql("mssql", "eatingstring", "exec web.checkofficeinfo @newid,@externip,@month;", dbparams).Rows.Count == 0)
                return new userModels() { client = new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" } } };
            return new userModels() { client = new Dictionary<string, object>() { { "clientid", clientJson["clientid"].ToString().TrimEnd() }, { "accesstoken", clientJson["accesstoken"].ToString().TrimEnd() }, { "email", "" } }, name = randomJson["username"].ToString().TrimEnd(), imageUrl = "https://images.unsplash.com/photo-1525253086316-d0c936c814f8" };
        }

        public Dictionary<string, object> checkAccess(string infotext)
        {
            if (string.IsNullOrWhiteSpace(infotext))
                return new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" }, { "email", "" } };
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@userid", infotext.Trim()));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec web.findclientoken @userid;", dbparams);
            if (mainRows.Rows.Count == 0)
                return new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" }, { "email", "" } };
            return new Dictionary<string, object>() { { "clientid", mainRows.Rows[0]["clientid"].ToString().TrimEnd() }, { "accesstoken", mainRows.Rows[0]["accesstoken"].ToString().TrimEnd() }, { "email", checkRandom(new Dictionary<string, object>() { { "clientid", mainRows.Rows[0]["clientid"].ToString().TrimEnd() }, { "accesstoken", mainRows.Rows[0]["accesstoken"].ToString().TrimEnd() } })["username"].ToString().TrimEnd() } };
        }

        public Dictionary<string, object> checkRandom(Dictionary<string, object> clientJson)
        {
            if (clientJson.Count == 0)
                return new Dictionary<string, object>() { { "random", "" }, { "username", "" } };
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@clientid", clientJson["clientid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@accesstoken", clientJson["accesstoken"].ToString().TrimEnd()));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec web.findsiteber @clientid,@accesstoken;", dbparams);
            if (mainRows.Rows.Count == 0)
                return new Dictionary<string, object>() { { "random", "" }, { "username", "" } };
            return new Dictionary<string, object>() { { "random", mainRows.Rows[0]["random"].ToString().TrimEnd() }, { "username", mainRows.Rows[0]["username"].ToString().TrimEnd() } };
        }
    }
}