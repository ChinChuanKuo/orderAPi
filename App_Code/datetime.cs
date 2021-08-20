using System;
using System.Collections.Generic;

namespace orderAPi.App_Code
{
    public class datetime
    {
        public string sqldate(string dataname, string sqlstring)
        {
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>(); // "select convert(varchar,getdate(),111);"
            switch (dataname)
            {
                case "mssql":
                    return database.selectMsSql(database.connectionString(sqlstring), "select convert(varchar,getdate(),111);", dbparamlist).Rows[0][0].ToString().TrimEnd();
                case "postgresql":
                    return database.selectPostgreSql(database.connectionString(sqlstring), "select to_char(now(),'YYYY/MM/DD');", dbparamlist).Rows[0][0].ToString().TrimEnd();
                default:
                    return null;
            }
        }

        public string sqltime(string dataname, string sqlstring)
        {
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();  //"select convert(varchar,getdate(),108);"
            switch (dataname)
            {
                case "mssql":
                    return database.selectMsSql(database.connectionString(sqlstring), "select convert(varchar,getdate(),108);", dbparamlist).Rows[0][0].ToString().TrimEnd();
                case "postgresql":
                    return database.selectPostgreSql(database.connectionString(sqlstring), "select to_char(now(),'HH:MM:SS');", dbparamlist).Rows[0][0].ToString().TrimEnd();
                default:
                    return null;
            }
        }

        public string differentime(string beforedate)
        {
            DateTime beforeDate = DateTime.Parse(beforedate), nowDate = DateTime.Now;
            TimeSpan ts = nowDate.Subtract(beforeDate);
            switch (ts.Days)
            {
                case 0:
                    switch (ts.Hours)
                    {
                        case 0:
                            switch (ts.Minutes)
                            {
                                case 0:
                                    return $"{ts.Seconds} secs";
                            }
                            return $"{ts.Minutes} ms";
                    }
                    return $"{ts.Hours} hs";
            }
            return $"{ts.Days} days";
        }

        public bool[] checkedBusiness(string ondate, string endate)
        {
            if (endate == "")
                return new bool[] { differentimeInt(ondate) >= 0, false };
            return new bool[] { false, true };
        }

        public string differentimeAbs(string beforedate)
        {
            DateTime beforeDate = DateTime.Parse(beforedate), nowDate = DateTime.Now;
            TimeSpan ts = nowDate.Subtract(beforeDate);
            switch (ts.Days)
            {
                case 0:
                    switch (ts.Hours)
                    {
                        case 0:
                            switch (ts.Minutes)
                            {
                                case 0:
                                    return $"{Math.Abs(ts.Seconds)} secs";
                            }
                            return $"{Math.Abs(ts.Minutes)} ms";
                    }
                    return $"{Math.Abs(ts.Hours)} hs";
            }
            return $"{Math.Abs(ts.Days)} days";
        }

        public int differentimeInt(string beforedate)
        {
            DateTime beforeDate = DateTime.Parse(beforedate), nowDate = DateTime.Now;
            TimeSpan ts = nowDate.Subtract(beforeDate);
            switch (ts.Days)
            {
                case 0:
                    switch (ts.Hours)
                    {
                        case 0:
                            switch (ts.Minutes)
                            {
                                case 0:
                                    return ts.Seconds;
                            }
                            return ts.Minutes;
                    }
                    return ts.Hours;
            }
            return ts.Days;
        }
    }
}