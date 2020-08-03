using System;
using System.Collections.Generic;
using System.Linq;

namespace NetJsonRpc.Protocol
{
    /// <summary>
    /// Wrapped list.
    /// </summary>
    public class WList
    {
        private IList<object> list;

        public WList()
        {
            list = new List<object>();
        }

        public WList(object source)
        {
            list = WUtil.ToList(source);

            if (list == null) list = new List<object>();
        }

        public IList<object> GetList()
        {
            return list;
        }

        public string GetString(int index)
        {
            return WUtil.ToString(list[index]);
        }

        public string GetString(int index, string defValue)
        {
            return WUtil.ToString(list[index], defValue);
        }

        public int GetInt(int index)
        {
            return WUtil.ToInt(list[index]);
        }

        public int GetInt(int index, int defValue)
        {
            return WUtil.ToInt(list[index], defValue);
        }

        public long GetLong(int index)
        {
            return WUtil.ToLong(list[index]);
        }

        public long GetLong(int index, long defValue)
        {
            return WUtil.ToLong(list[index], defValue);
        }

        public double GetDouble(int index)
        {
            return WUtil.ToDouble(list[index]);
        }

        public double GetDouble(int index, double defValue)
        {
            return WUtil.ToDouble(list[index], defValue);
        }

        public DateTime GetDateTime(int index)
        {
            return WUtil.ToDateTime(list[index]);
        }

        public DateTime GetDateTime(int index, DateTime defValue)
        {
            return WUtil.ToDateTime(list[index], defValue);
        }

        public object[] GetArray(int index, bool notNull = false)
        {
            object value = list[index];

            if(value != null)
            {
                return WUtil.ToArray(value);
            }

            if (notNull)
            {
                return new object[0];
            }

            return null;
        }

        public IList<object> GetList(int index, bool notNull = false)
        {
            object value = list[index];

            if (value != null)
            {
                return WUtil.ToList(value);
            }

            if (notNull)
            {
                return new List<object>();
            }

            return null;
        }

        public IDictionary<string, object> GetDictionary(int index, bool notNull = false)
        {
            object value = list[index];

            if (value != null)
            {
                return WUtil.ToDictionary(value);
            }

            if (notNull)
            {
                return new Dictionary<string, object>();
            }

            return null;
        }

        public bool IsBlank(int index)
        {
            string value = WUtil.ToString(list[index]);

            if (value != null && value.Length != 0)
            {
                return false;
            }

            return true;
        }

        public int Count()
        {
            return list.Count();
        }

        public bool IsEmpty()
        {
            return list.Count == 0;
        }
    }
}
