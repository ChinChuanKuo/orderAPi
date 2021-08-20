using System.Collections.Generic;
using System.Text.Json;
using System.Data;
using orderAPi.App_Code;

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
            mainRows = database.checkSelectSql("mssql", "eatingstring", "exec web.checksiteber @userid,@password;", dbparams);
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
    }
}