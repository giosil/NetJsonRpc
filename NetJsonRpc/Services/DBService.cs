using System;
using System.Collections.Generic;
using System.Data.Common;

using NetJsonRpc.Protocol;

using MySqlConnector;

namespace NetJsonRpc.Services
{
    public class DBService
    {
        public IList<IDictionary<string, object>> FindCities(IDictionary<string, object> filter)
        {
            IList<IDictionary<string, object>> listResult = new List<IDictionary<string, object>>();

            WMap wmFilter = new WMap(filter);

            string countryCode = wmFilter.GetString("countryCode");

            if (countryCode == null || countryCode.Length == 0)
            {
                return listResult;
            }

            string sSQL = "SELECT ID,NAME FROM CITY WHERE COUNTRYCODE='" + countryCode.Replace("'", "''") + "' ORDER BY NAME";

            DbConnection conn = null;
            DbCommand cmd = null;
            DbDataReader dr = null;
            try
            {
                conn = GetDefaultConnection();

                cmd = CreateCommand(sSQL, conn);

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    IDictionary<string, object> record = new Dictionary<string, object>();
                    record["id"] = dr[0];
                    record["name"] = dr[1];

                    listResult.Add(record);
                }
            }
            finally
            {
                if (dr != null) dr.Close();
                if (conn != null) conn.Close();
            }

            return listResult;
        }

        public IList<IDictionary<string, object>> FindCities2(IDictionary<string, object> filter)
        {
            IList<IDictionary<string, object>> listResult = new List<IDictionary<string, object>>();

            WMap wmFilter = new WMap(filter);

            string countryCode = wmFilter.GetString("countryCode");

            if (countryCode == null || countryCode.Length == 0)
            {
                return listResult;
            }

            string sSQL = "SELECT ID,NAME FROM CITY WHERE COUNTRYCODE=@countryCode ORDER BY NAME";

            DbConnection conn = null;
            DbCommand cmd = null;
            DbDataReader dr = null;
            try
            {
                conn = GetDefaultConnection();

                cmd = CreateCommand(sSQL, conn);

                cmd.Prepare();
                AddParameter(cmd, "@countryCode", countryCode);

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    IDictionary<string, object> record = new Dictionary<string, object>();
                    record["id"] = dr[0];
                    record["name"] = dr[1];

                    listResult.Add(record);
                }
            }
            finally
            {
                if (dr != null) dr.Close();
                if (conn != null) conn.Close();
            }

            return listResult;
        }

        public int InsertLang(string countryCode, string language, bool isOfficial, int percentage)
        {
            string sSQL = "INSERT INTO COUNTRYLANGUAGE(COUNTRYCODE,LANGUAGE,ISOFFICIAL,PERCENTAGE) ";
            sSQL += "VALUES(@countryCode, @language, @isOfficial, @percentage)";

            int result = 0;

            DbConnection conn = null;
            DbCommand cmd = null;
            DbTransaction trs = null;
            try
            {
                conn = GetDefaultConnection();

                trs = conn.BeginTransaction();

                cmd = CreateCommand(sSQL, conn, trs);

                cmd.Prepare();
                AddParameter(cmd, "@countryCode", countryCode);
                AddParameter(cmd, "@language", language);
                AddParameter(cmd, "@isOfficial", isOfficial);
                AddParameter(cmd, "@percentage", percentage);

                result = cmd.ExecuteNonQuery();

                trs.Commit();
            }
            catch(Exception ex)
            {
                if(trs != null) trs.Rollback();
                throw ex;
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return result;
        }

        DbCommand AddParameter(DbCommand cmd, string name, String value)
        {
            DbParameter parameter = new MySqlParameter(name, MySqlDbType.String);
            parameter.Value = value;
            cmd.Parameters.Add(parameter);
            return cmd;
        }

        DbCommand AddParameter(DbCommand cmd, string name, int value)
        {
            DbParameter parameter = new MySqlParameter(name, MySqlDbType.Int32);
            parameter.Value = value;
            cmd.Parameters.Add(parameter);
            return cmd;
        }

        DbCommand AddParameter(DbCommand cmd, string name, bool value)
        {
            DbParameter parameter = new MySqlParameter(name, MySqlDbType.Enum);
            parameter.Value = value ? 'T' : 'F';
            cmd.Parameters.Add(parameter);
            return cmd;
        }

        DbCommand CreateCommand(string sSQL, DbConnection conn)
        {
            return new MySqlCommand(sSQL, (MySqlConnection) conn);
        }

        DbCommand CreateCommand(string sSQL, DbConnection conn, DbTransaction trs)
        {
            return new MySqlCommand(sSQL, (MySqlConnection)conn, (MySqlTransaction)trs);
        }

        DbConnection GetDefaultConnection()
        {
            DbConnection connection = new MySqlConnection("server=localhost;port=3306;user=root;password=ised;database=world");

            connection.Open();

            return connection;
        }
    }
}
