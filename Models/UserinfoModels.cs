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
            switch (new database().checkSelectSql("mssql", "eatingstring", "exec web.checkuserinfo @newid,@externip;", dbparams).Rows.Count)
            {
                case 0:
                    return new userModels() { client = new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" } } };
            }
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
            switch (new database().checkSelectSql("mssql", "eatingstring", "exec web.checkuserinfo @newid,@externip;", dbparams).Rows.Count)
            {
                case 0:
                    return new userModels() { client = new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" } } };
            }
            return new userModels() { client = new Dictionary<string, object>() { { "clientid", clientJson["clientid"].ToString().TrimEnd() }, { "accesstoken", clientJson["accesstoken"].ToString().TrimEnd() }, { "email", "" } }, name = randomJson["username"].ToString().TrimEnd(), imageUrl = "https://images.unsplash.com/photo-1525253086316-d0c936c814f8" };
        }

        /*public Dictionary<string, object> GetAccessModels(string clientinfo, string deviceinfo, string infotext, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@userid", infotext.Trim()));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec web.findclientoken @userid;", dbparams);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" }, { "email", "" } };
            }
            return new Dictionary<string, object>() { { "clientid", mainRows.Rows[0]["clientid"].ToString().TrimEnd() }, { "accesstoken", mainRows.Rows[0]["accesstoken"].ToString().TrimEnd() }, { "email", checkRandom(new Dictionary<string, object>() { { "clientid", mainRows.Rows[0]["clientid"].ToString().TrimEnd() }, { "accesstoken", mainRows.Rows[0]["accesstoken"].ToString().TrimEnd() } })["username"].ToString().TrimEnd() } };
        }*/

        public Dictionary<string, object> checkAccess(string infotext)
        {
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@userid", infotext.Trim()));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec web.findclientoken @userid;", dbparams); 
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new Dictionary<string, object>() { { "clientid", "" }, { "accesstoken", "" }, { "email", "" } };
            }
            return new Dictionary<string, object>() { { "clientid", mainRows.Rows[0]["clientid"].ToString().TrimEnd() }, { "accesstoken", mainRows.Rows[0]["accesstoken"].ToString().TrimEnd() }, { "email", checkRandom(new Dictionary<string, object>() { { "clientid", mainRows.Rows[0]["clientid"].ToString().TrimEnd() }, { "accesstoken", mainRows.Rows[0]["accesstoken"].ToString().TrimEnd() } })["username"].ToString().TrimEnd() } };
        }

        public Dictionary<string, object> checkRandom(Dictionary<string, object> clientJson)
        {
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@clientid", clientJson["clientid"].ToString().TrimEnd()));
            dbparams.Add(new dbparam("@accesstoken", clientJson["accesstoken"].ToString().TrimEnd()));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec web.findsiteber @clientid,@accesstoken;", dbparams);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new Dictionary<string, object>() { { "random", "" }, { "username", "" } };
            }
            return new Dictionary<string, object>() { { "random", mainRows.Rows[0]["random"].ToString().TrimEnd() }, { "username", mainRows.Rows[0]["username"].ToString().TrimEnd() } };
        }
    }
}