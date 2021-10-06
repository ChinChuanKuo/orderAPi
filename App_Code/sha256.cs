using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace orderAPi.App_Code
{
    public class sha256
    {
        public string encry256(string encry)
        {
            if (string.IsNullOrWhiteSpace(encry)) return "";
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.Default.GetBytes(encry.TrimEnd())));
        }

        public string new256(string dataname, string sqlstring)
        {
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();
            switch (dataname)
            {
                case "mssql":
                    return database.selectMsSql(database.connectionString(sqlstring), "select NEWID();", dbparamlist).Rows[0][0].ToString().TrimEnd();
                case "postgresql":
                    return database.selectPostgreSql(database.connectionString(sqlstring), "select uuid_generate_v4();", dbparamlist).Rows[0][0].ToString().TrimEnd();
                default:
                    return null;
            }
        }

        public string[] substr256(string datakey, int length)
        {
            return new string[] { datakey.Substring(length).Trim(), datakey.Substring(0, length).Trim() };
        }
    }
}