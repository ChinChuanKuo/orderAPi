using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using orderAPi.App_Code;

namespace orderAPi.Models
{
    public class NotificationsClass
    {
        public List<Dictionary<string, object>> GetSearchModels(string clientinfo, string deviceinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.noticeform @newid;", dbparams).Rows)
            {
                var messengeJson = JsonSerializer.Deserialize<Dictionary<string, object>>(dr["messenge"].ToString().TrimEnd());
                items.Add(new Dictionary<string, object>() { { "notice", new Dictionary<string, object>() { { "imageUrl", messengeJson["imageUrl"].ToString().TrimEnd() }, { "message", messengeJson["message"].ToString().TrimEnd() }, { "timeAgo", new datetime().differentime($"{dr["indate"].ToString().TrimEnd()} {dr["intime"].ToString().TrimEnd()}") } } }, { "requireid", new Dictionary<string, object>() { { "orderid", dr["noticeid"].ToString().TrimEnd() } } } });
            }
            return items;
        }

        public List<Dictionary<string, object>> GetFilterModels(string clientinfo, string deviceinfo, string lengthinfo, string cuurip)
        {
            var clientJson = JsonSerializer.Deserialize<Dictionary<string, object>>(clientinfo);
            var deviceJson = JsonSerializer.Deserialize<Dictionary<string, object>>(deviceinfo);
            var lengthJson = JsonSerializer.Deserialize<Dictionary<string, object>>(lengthinfo);
            var randomJson = new UserinfoClass().checkRandom(clientJson);
            int index = int.Parse(lengthJson["length"].ToString().TrimEnd()) / 10;
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", new sha256().encry256($"{clientJson["clientid"].ToString().TrimEnd()}{randomJson["random"].ToString().TrimEnd()}{clientJson["accesstoken"].ToString().TrimEnd()}")));
            dbparams.Add(new dbparam("@startid", index + 10 * index));
            dbparams.Add(new dbparam("@endid", index + 10 * (index + 1)));
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in new database().checkSelectSql("mssql", "eatingstring", "exec eat.noticeform @newid,@startid,@endid;", dbparams).Rows)
            {
                var messengeJson = JsonSerializer.Deserialize<Dictionary<string, object>>(dr["messenge"].ToString().TrimEnd());
                items.Add(new Dictionary<string, object>() { { "notice", new Dictionary<string, object>() { { "imageUrl", messengeJson["imageUrl"].ToString().TrimEnd() }, { "message", messengeJson["message"].ToString().TrimEnd() }, { "timeAgo", new datetime().differentime($"{dr["indate"].ToString().TrimEnd()} {dr["intime"].ToString().TrimEnd()}") } } }, { "requireid", new Dictionary<string, object>() { { "orderid", dr["noticeid"].ToString().TrimEnd() } } } });
            }
            return items;
        }

        public bool noticeModels(string notoper, Dictionary<string, object> messenge, string newid)
        {
            List<dbparam> dbparams = new List<dbparam>();
            dbparams.Add(new dbparam("@newid", notoper));
            dbparams.Add(new dbparam("@messenge", JsonSerializer.Serialize(messenge)));
            dbparams.Add(new dbparam("@inoper", newid));
            return new database().checkActiveSql("mssql", "eatingstring", "exec eat.insertnotice @newid,@messenge,@inoper;", dbparams) == "istrue";
        }
    }
}