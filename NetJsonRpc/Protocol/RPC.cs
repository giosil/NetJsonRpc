using System;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json.Linq;

namespace NetJsonRpc.Protocol
{
    public static class RPC
    {
        private static IDictionary<string, object> handlers = new Dictionary<string, object>();

        public static void AddHanlder(string handlerId, object hanlder)
        {
            handlers[handlerId] = hanlder;
        }

        public static RPCResponse Invoke(IDictionary<string, object> request)
        {
            WMap wmap = new WMap(request);

            int id = wmap.GetInt("id");

            string method = wmap.GetString("method");

            object[] parameters = wmap.GetArray("params", true);

            if (method == null || method.Length == 0)
            {
                return new RPCResponse(id, -32600, "Invalid Request");
            }

            int sep = method.IndexOf('.');

            if(sep <= 0)
            {
                return new RPCResponse(id, -32600, "Invalid Request");
            }

            string handlerId = method.Substring(0, sep);

            if(!handlers.ContainsKey(handlerId))
            {
                return new RPCResponse(id, -32600, "Handler not available");
            }

            object target = handlers[handlerId];

            string methodName = method.Substring(sep + 1);

            return Invoke(id, target, methodName, parameters);
        }

        public static RPCResponse Invoke(int id, object target, string methodName, object[] parameters)
        {
            if (target == null)
            {
                return new RPCResponse(id, -32600, "Handler not available");
            }

            if (methodName == null || methodName.Length == 0)
            {
                return new RPCResponse(id, -32600, "Invalid Request");
            }

            if (parameters == null)
            {
                parameters = new object[0];
            }

            string methodNameU = methodName.Substring(0, 1).ToUpper() + methodName.Substring(1);

            object result = null;

            Type type = target.GetType();

            MethodInfo[] arrayOfMethodInfo = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (MethodInfo methodInfo in arrayOfMethodInfo)
            {
                if (methodName.Equals(methodInfo.Name) || methodNameU.Equals(methodInfo.Name))
                {
                    ParameterInfo[] arrayOfParameterInfo = methodInfo.GetParameters();

                    if (arrayOfParameterInfo.Length == parameters.Length)
                    {
                        object[] invokeParameters = null;

                        try
                        {
                            invokeParameters = GetParameters(arrayOfParameterInfo, parameters);
                        }
                        catch (Exception ex)
                        {
                            return new RPCResponse(id, -32600, ex.ToString());
                        }

                        if (invokeParameters == null) continue;

                        try
                        {
                            result = methodInfo.Invoke(target, invokeParameters);
                        }
                        catch (Exception ex)
                        {
                            return new RPCResponse(id, -32000, ex.ToString());
                        }

                        return new RPCResponse(id, result);
                    }
                }
            }

            return new RPCResponse(id, -32601, "Method not found");
        }

        public static object[] GetParameters(ParameterInfo[] arrayOfParameterInfo, object[] parameters)
        {
            if (arrayOfParameterInfo.Length != parameters.Length) return null;

            for (int i = 0; i < arrayOfParameterInfo.Length; i++)
            {
                object value = parameters[i];

                if (value == null) continue;

                ParameterInfo parameterInfo = arrayOfParameterInfo[i];

                Type typePar = parameterInfo.ParameterType;

                if(value is JValue)
                {
                    value = ((JValue)value).Value;

                    parameters[i] = value;
                }

                Type typeObj = value.GetType();

                if (!typePar.IsAssignableFrom(typeObj))
                {
                    var typeParName = typePar.Namespace + "." + typePar.Name;

                    if (!typeParName.StartsWith("System."))
                    {
                        // IDictionary -> Data Class

                        if (value is IDictionary<string, object>)
                        {
                            parameters[i] = WUtil.CreateObject((IDictionary<string, object>)value, typePar);
                            continue;
                        }
                        else if(value is JObject)
                        {
                            parameters[i] = WUtil.CreateObject((JObject)value, typePar);
                            continue;
                        }
                    }
                    else if (typeof(IDictionary<string, object>).IsAssignableFrom(typePar))
                    {
                        // Data Class -> IDictionary

                        var typeObjName = typeObj.Namespace + "." + typeObj.Name;

                        if (value is IDictionary<string, object>)
                        {
                            continue;
                        }
                        else if (value is JObject)
                        {
                            parameters[i] = WUtil.ToDictionary(value);
                            continue;
                        }
                        else if (!typeObjName.StartsWith("System."))
                        {
                            parameters[i] = WUtil.ToDictionary(value);
                            continue;
                        }
                    }
                    else if (typeof(Array).IsAssignableFrom(typePar))
                    {
                        if (value is IEnumerable<object>)
                        {
                            parameters[i] = WUtil.ToArray(value);
                            continue;
                        }
                    }
                    else if (typeof(IList<object>).IsAssignableFrom(typePar))
                    {
                        if (value is IEnumerable<object>)
                        {
                            parameters[i] = WUtil.ToList(value);
                            continue;
                        }
                    }
                    else if (value is Int64)
                    {
                        if (typePar.Equals(typeof(Int32)))
                        {
                            parameters[i] = Convert.ToInt32(value);
                            continue;
                        }
                    }
                    return null;
                }
            }

            return parameters;
        }
    }
}
