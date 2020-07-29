using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            if(countryCode == null || countryCode.Length == 0)
            {
                return listResult;
            }

            string sSQL = "SELECT ID,NAME FROM CITY WHERE COUNTRYCODE='" + countryCode.Replace("'", "''") + "' ORDER BY NAME";

            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlDataReader dr = null;
            try
            {
                conn = GetDefaultConnection();

                cmd = new MySqlCommand(sSQL, conn);

                dr = cmd.ExecuteReader();

                while(dr.Read())
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

        MySqlConnection GetDefaultConnection()
        {
            MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;user=root;password=ised;database=world");

            connection.Open();

            return connection;
        }
    }
}
