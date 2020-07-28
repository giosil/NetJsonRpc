using System;
using System.Collections.Generic;
using System.Linq;

namespace NetJsonRpc.Protocol
{
    public class WMap
    {
        private IDictionary<string, object> map;

        public WMap()
        {
            map = new Dictionary<string, object>();
        }

        public WMap(object source)
        {
            map = WUtil.ToDictionary(source);

            if (map == null) map = new Dictionary<string, object>();
        }

        public IDictionary<string, object> GetDictionary()
        {
            return map;
        }

        public string GetString(string key)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToString(map[key]);
            }

            return null;
        }

        public string GetString(string key, string defValue)
        {
            if (map.ContainsKey(key))
            {
                string result = WUtil.ToString(map[key]);

                if (result == null) return defValue;

                return result;
            }

            return defValue;
        }

        public int GetInt(string key)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToInt(map[key]);
            }

            return 0;
        }

        public int GetInt(string key, int defValue)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToInt(map[key]);
            }

            return defValue;
        }

        public long GetLong(string key)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToLong(map[key]);
            }

            return 0l;
        }

        public long GetLong(string key, long defValue)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToLong(map[key]);
            }

            return defValue;
        }

        public double GetDouble(string key)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToDouble(map[key]);
            }

            return 0.0d;
        }

        public double GetDouble(string key, double defValue)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToDouble(map[key]);
            }

            return defValue;
        }

        public DateTime GetDateTime(string key)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToDateTime(map[key]);
            }

            return DateTime.MinValue;
        }

        public DateTime GetDateTime(string key, DateTime defValue)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToDateTime(map[key]);
            }

            return defValue;
        }

        public object[] GetArray(string key, bool notNull = false)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToArray(map[key]);
            }

            if (notNull)
            {
                return new object[0];
            }

            return null;
        }

        public IList<object> GetList(string key, bool notNull = false)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToList(map[key]);
            }

            if (notNull)
            {
                return new List<object>();
            }

            return null;
        }

        public IDictionary<string, object> GetDictionary(string key, bool notNull = false)
        {
            if (map.ContainsKey(key))
            {
                return WUtil.ToDictionary(map[key]);
            }

            if (notNull)
            {
                return new Dictionary<string, object>();
            }

            return null;
        }

        public bool ContainsKey(string key)
        {
            return map.ContainsKey(key);
        }

        public bool IsBlank(string key)
        {
            if (map.ContainsKey(key))
            {
                string value = WUtil.ToString(map[key]);

                if (value != null && value.Length != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public int Count()
        {
            return map.Count();
        }

        public bool IsEmpty()
        {
            return map.Count == 0;
        }
    }
}
