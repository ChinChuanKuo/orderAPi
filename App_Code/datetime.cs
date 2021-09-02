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

        public virtual string differentime(string begindate)
        {
            DateTime beforeDate = DateTime.Parse(begindate), 
            nowDate = DateTime.Now;
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
                                    return $"{ts.Seconds} sec";
                            }
                            return $"{ts.Minutes} min";
                    }
                    return $"{ts.Hours} hr";
            }
            return $"{ts.Days} d";
        }

        public bool[] checkedBusiness(string ondate, string endate)
        {
            if (endate == "")
                return new bool[] { differtime(ondate) >= 0, false };
            return new bool[] { false, true };
        }

        public int differtime(string begindate)
        {
            DateTime beforeDate = DateTime.Parse(begindate),
            nowDate = DateTime.Now;
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

    public class abstime : datetime
    {
        public override string differentime(string begindate)
        {
            DateTime beforeDate = DateTime.Parse(begindate),
            nowDate = DateTime.Now;
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
                                    return $"{Math.Abs(ts.Seconds)} sec";
                            }
                            return $"{Math.Abs(ts.Minutes)} min";
                    }
                    return $"{Math.Abs(ts.Hours)} hr";
            }
            return $"{Math.Abs(ts.Days)} d";
        }
    }
}