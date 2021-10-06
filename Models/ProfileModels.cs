using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class ProfileClass
    {
        public Dictionary<string, object> GetSearchModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            DataTable mainRows = new DataTable();
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@externip", cuurip));
            mainRows = new database().checkSelectSql("mssql", "eatingstring", "exec web.findsiteinfo @newid,@externip;", dbparams);
            if (mainRows.Rows.Count == 0)
                return new Dictionary<string, object>() { };
            return new Dictionary<string, object>() { { "sign", JsonSerializer.Deserialize<Dictionary<string, object>>(mainRows.Rows[0]["signinfo"].ToString().TrimEnd()) }, { "device", JsonSerializer.Deserialize<Dictionary<string, object>>(mainRows.Rows[0]["device"].ToString().TrimEnd()) } };
        }
    }
}