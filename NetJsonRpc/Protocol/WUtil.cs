﻿using System;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json.Linq;

namespace NetJsonRpc.Protocol
{
    /// <summary>
    /// Utilities.
    /// </summary>
    public static class WUtil
    {
        public static string ToString(object value)
        {
            if (value == null) return null;

            return value.ToString();
        }

        public static string ToString(object value, string defValue)
        {
            if (value == null) return defValue;

            return value.ToString();
        }

        public static int ToInt(object value)
        {
            if (value == null) return 0;

            if (value is Int32)
            {
                return (Int32)value;
            }

            return Convert.ToInt32(value);
        }

        public static int ToInt(object value, int defValue)
        {
            if (value == null) return defValue;

            if (value is Int32)
            {
                return (Int32)value;
            }

            return Convert.ToInt32(value);
        }

        public static long ToLong(object value)
        {
            if (value == null) return 0L;

            if (value is Int64)
            {
                return (Int64)value;
            }

            return Convert.ToInt64(value);
        }

        public static long ToLong(object value, long defValue)
        {
            if (value == null) return defValue;

            if (value is Int64)
            {
                return (Int64)value;
            }

            return Convert.ToInt64(value);
        }

        public static double ToDouble(object value)
        {
            if (value == null) return 0.0;

            if (value is Double)
            {
                return (Double)value;
            }

            return Convert.ToDouble(value);
        }

        public static double ToDouble(object value, double defValue)
        {
            if (value == null) return defValue;

            if (value is Double)
            {
                return (Double)value;
            }

            return Convert.ToDouble(value);
        }

        public static bool ToBoolean(object value)
        {
            if (value == null) return false;

            if(value is Boolean)
            {
                return (Boolean)value;
            }

            return Convert.ToBoolean(value);
        }

        public static bool ToBoolean(object value, bool defValue)
        {
            if (value == null) return defValue;

            if (value is Boolean)
            {
                return (Boolean)value;
            }

            return Convert.ToBoolean(value);
        }

        public static DateTime ToDateTime(object value)
        {
            if(value is DateTime)
            {
                return (DateTime)value;
            }
            else if(value is string)
            {
                return ParseDateTime((string)value, DateTime.MinValue);
            }
            else if (value is Int32)
            {
                return ParseDateTime((Int32)value, DateTime.MinValue);
            }

            return Convert.ToDateTime(value);
        }

        public static DateTime ToDateTime(object value, DateTime defValue)
        {
            if (value == null) return defValue;

            if (value is DateTime)
            {
                return (DateTime)value;
            }
            else if (value is string)
            {
                return ParseDateTime((string)value, defValue);
            }
            else if (value is Int32)
            {
                return ParseDateTime((Int32)value, defValue);
            }

            return Convert.ToDateTime(value);
        }

        public static IDictionary<string, object> ToDictionary(object source)
        {
            if (source == null) return null;

            if (source is IDictionary<string, object>)
            {
                return (IDictionary<string, object>)source;
            }
            else if(source is WMap)
            {
                return ((WMap)source).GetDictionary();
            }

            IDictionary<string, object> result = new Dictionary<string, object>();

            if (source is JObject)
            {
                IEnumerator<KeyValuePair<string, JToken>> enumerator = ((JObject)source).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, JToken> kvp = enumerator.Current;
                    result[kvp.Key] = ToObject(kvp.Value);
                }
                return result;
            }

            PropertyInfo[] arrayOfPropertyInfo = source.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in arrayOfPropertyInfo)
            {
                result[propertyInfo.Name] = propertyInfo.GetValue(source);
            }

            return result;
        }

        public static object[] ToArray(object value)
        {
            if (value == null) return null;

            if (value is WList) value = ((WList)value).GetList();
            if (value is Array) return (object[]) value;

            object[] result = null;
            if (typeof(IEnumerable<object>).IsAssignableFrom(value.GetType()))
            {
                IList<object> list = new List<object>();
                IEnumerator<object> enumerator = ((IEnumerable<object>)value).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    list.Add(enumerator.Current);
                }
                result = new object[list.Count];
                list.CopyTo(result, 0);
                return result;
            }
            result = new object[1];
            result[0] = value;
            return result;
        }

        public static Array ToArrayOf(object value, Type typeElement)
        {
            if (value == null) return null;

            if(typeElement == null) typeElement = typeof(object);

            if (value is WList) value = ((WList)value).GetList();

            Array result = null;
            if (typeof(IEnumerable<object>).IsAssignableFrom(value.GetType()))
            {
                IList<object> list = new List<object>();
                IEnumerator<object> enumerator = ((IEnumerable<object>)value).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    list.Add(ToObjectOf(enumerator.Current, typeElement));
                }
                result = Array.CreateInstance(typeElement, list.Count);
                for(int i = 0; i < list.Count; i++)
                {
                    result.SetValue(list[i], i);
                }                
                return result;
            }
            result = Array.CreateInstance(typeElement, 1);
            result.SetValue(ToObjectOf(value, typeElement), 0);
            return result;
        }

        public static object ToObjectOf(object value, Type typeElement)
        {
            if (value == null) return null;

            if (typeElement == null) return value;

            string typeName = typeElement.Namespace + "." + typeElement.Name;
            if(typeName.EndsWith(']'))
            {
                return ToArrayOf(value, typeElement.GetElementType());
            }
            else if (!typeName.StartsWith("System."))
            {
                return CreateObject(ToDictionary(value), typeElement);
            }
            else if (typeName.Equals("System.String"))
            {
                return ToString(value);
            }
            else if (typeName.Equals("System.Int32"))
            {
                return ToInt(value);
            }
            else if (typeName.Equals("System.Int64"))
            {
                return ToLong(value);
            }
            else if (typeName.Equals("System.Double"))
            {
                return ToDouble(value);
            }
            else if (typeName.Equals("System.Boolean"))
            {
                return ToBoolean(value);
            }
            else if (typeName.Equals("System.DateTime"))
            {
                return ToDateTime(value);
            }
            else if (typeof(IDictionary<string, object>).IsAssignableFrom(typeElement))
            {
                return ToDictionary(value);
            }
            else if (typeof(Array).IsAssignableFrom(typeElement))
            {
                return ToArray(value);
            }
            else if (typeof(IList<object>).IsAssignableFrom(typeElement))
            {
                return ToList(value);
            }
            return value;
        }

        public static IList<object> ToList(object value)
        {
            if (value == null) return null;

            if(value is WList) value = ((WList)value).GetList();
            if (value is List<object>) return (IList<object>)value;

            IList<object> result = new List<object>();
            if (typeof(IEnumerable<object>).IsAssignableFrom(value.GetType()))
            {
                IEnumerator<object> enumerator = ((IEnumerable<object>)value).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    result.Add(enumerator.Current);
                }
                return result;
            }
            result.Add(value);
            return result;
        }

        public static object ToListOf(object value, Type typeElement)
        {
            if (value == null) return null;

            if (value is WList) value = ((WList)value).GetList();

            if (typeElement == null) typeElement = typeof(object);

            var typeGen = typeof(List<>).MakeGenericType(typeElement);

            var result = Activator.CreateInstance(typeGen);

            MethodInfo addMethod = result.GetType().GetMethod("Add");

            if (addMethod == null) return null;

            object[] addParams = new object[1];

            if (typeof(IEnumerable<object>).IsAssignableFrom(value.GetType()))
            {
                IEnumerator<object> enumerator = ((IEnumerable<object>)value).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    addParams[0] = ToObjectOf(enumerator.Current, typeElement);
                    addMethod.Invoke(result, addParams);
                }
                return result;
            }
            addParams[0] = ToObjectOf(value, typeElement);
            addMethod.Invoke(result, addParams);
            return result;
        }

        public static object CreateObject(IDictionary<string, object> dictionary, Type type)
        {
            if (dictionary == null) return null;
            if(type == null) return dictionary;

            JObject jobject = JObject.FromObject(dictionary);

            return CreateObject(jobject, type);
        }

        public static object CreateObject(JObject jobject, Type type)
        {
            if (jobject == null) return null;

            if(type == null) return ToDictionary(jobject);

            object result = Activator.CreateInstance(type);

            foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
            {
                var key = keyValuePair.Key;

                PropertyInfo propertyInfo = type.GetProperty(key);

                if (propertyInfo == null)
                {
                    key = key.Substring(0, 1).ToUpper() + key.Substring(1);

                    propertyInfo = type.GetProperty(key);
                }

                if (propertyInfo == null) continue;

                Type propertyType = propertyInfo.PropertyType;

                string propertyTypeName = propertyType.Namespace + "." + propertyType.Name;

                if(propertyTypeName.EndsWith(']'))
                {
                    Type elementType = propertyType.GetElementType();

                    object arrayOf = ToArrayOf(ToObject(keyValuePair.Value), elementType);

                    propertyInfo.SetValue(result, arrayOf);
                }
                else if (!propertyTypeName.StartsWith("System."))
                {
                    IDictionary<string, object> dicValue = ToDictionary(ToObject(keyValuePair.Value));

                    propertyInfo.SetValue(result, CreateObject(dicValue, propertyType));
                }
                else if(propertyTypeName.Equals("System.DateTime"))
                {
                    object value = ToObject(keyValuePair.Value);

                    propertyInfo.SetValue(result, ToDateTime(value));
                }
                else
                {
                    propertyInfo.SetValue(result, keyValuePair.Value.ToObject(propertyType));
                }
            }

            return result;
        }

        public static DateTime ParseDateTime(int value, DateTime defValue)
        {
            if (value < 10000) return defValue;

            int year = value / 10000;
            int month = (value % 10000) / 100;
            int day = (value % 10000) % 100;

            return new DateTime(year, month, day);
        }

        public static DateTime ParseDateTime(string text, DateTime defValue)
        {
            if(text == null || text.Length == 0)
            {
                return defValue;
            }
            if(text.Length > 9 && text[4] == '-' && text[7] == '-')
            {
                // ISO 8601
                return DateTime.Parse(text);
            }

            List<int> tokens = new List<int>();

            string number = "";
            foreach(char c in text)
            {
                if(Char.IsDigit(c))
                {
                    number += c;
                }
                else if(number.Length > 0)
                {
                    tokens.Add(Convert.ToInt32(number));

                    number = "";
                }
            }
            if (number.Length > 0)
            {
                tokens.Add(Convert.ToInt32(number));
            }
            if (tokens.Count < 3)
            {
                return defValue;
            }

            int day = tokens[0] < 32 ? tokens[0] : tokens[2];
            int month = tokens[1];
            int year = tokens[0] < 32 ? tokens[2] : tokens[0];
            int hour = tokens.Count > 3 ? tokens[3] : 0;
            int minute = tokens.Count > 4 ? tokens[4] : 0;
            int second = tokens.Count > 5 ? tokens[5] : 0;

            return new DateTime(year, month, day, hour, minute, second);
        }

        public static object ToObject(JToken jtoken)
        {
            if (jtoken == null) return null;
            try
            {
                switch (jtoken.Type)
                {
                    case JTokenType.None:
                        return jtoken.ToObject<string>();

                    case JTokenType.Object:
                        return jtoken.ToObject<IDictionary<string, object>>();

                    case JTokenType.Array:
                        return jtoken.ToObject<IList<object>>();

                    case JTokenType.Constructor:
                        return jtoken.ToObject<string>();

                    case JTokenType.Property:
                        return jtoken.ToObject<string>();

                    case JTokenType.Comment:
                        return jtoken.ToObject<string>();

                    case JTokenType.Integer:
                        try
                        {
                            return jtoken.ToObject<int>();
                        }
                        catch (System.OverflowException)
                        {
                            return jtoken.ToObject<long>();
                        }
                    case JTokenType.Float:
                        return jtoken.ToObject<double>();

                    case JTokenType.String:
                        return jtoken.ToObject<string>();

                    case JTokenType.Boolean:
                        return jtoken.ToObject<bool>();

                    case JTokenType.Null:
                        return null;

                    case JTokenType.Undefined:
                        return null;

                    case JTokenType.Date:
                        return jtoken.ToObject<DateTime>();

                    case JTokenType.Raw:
                        return jtoken.ToObject<string>();

                    case JTokenType.Bytes:
                        return jtoken.ToObject<string>();

                    case JTokenType.Guid:
                        return jtoken.ToObject<string>();

                    case JTokenType.Uri:
                        return jtoken.ToObject<string>();

                    case JTokenType.TimeSpan:
                        return jtoken.ToObject<DateTime>();
                }
            }
            catch (Exception)
            {
            }
            return jtoken.ToObject<string>();
        }
    }
}
