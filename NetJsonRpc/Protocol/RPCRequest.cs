using System;
using System.Collections.Generic;

namespace NetJsonRpc.Protocol
{
    public class RPCRequest
    {
        private int id;
        private string method;
        private object[] parameters;

        public RPCRequest()
        {
        }

        public RPCRequest(int id, string method)
        {
            this.id = id;
            this.method = method;
            this.parameters = new object[0];
        }

        public RPCRequest(int id, string method, object[] parameters)
        {
            this.id = id;
            this.method = method;
            this.parameters = parameters;
            if(this.parameters == null)
            {
                this.parameters = new object[0];
            }
        }

        public RPCRequest(IDictionary<string, object> request)
        {
            WMap wmap = new WMap(request);

            this.id = wmap.GetInt("id");

            this.method = wmap.GetString("method");

            this.parameters = wmap.GetArray("params", true);
        }

        public int Id { get => id; set => id = value; }
        public string Method { get => method; set => method = value; }
        public object[] Parameters { get => parameters; set => parameters = value; }

        public string GetHandlerName()
        {
            if (this.method == null) return "";

            int sep = this.method.IndexOf('.');

            if(sep > 0)
            {
                return this.method.Substring(0, sep);
            }

            return "";
        }

        public string GetMethodName()
        {
            if (this.method == null) return "";

            int sep = this.method.IndexOf('.');

            if (sep > 0)
            {
                return this.method.Substring(sep + 1);
            }

            return this.method;
        }

        public Dictionary<string, object> GetDictionary()
        {
            Dictionary<string, object> request = new Dictionary<string, object>();
            request["jsonrpc"] = "2.0";
            request["id"] = id;
            request["method"] = method;
            request["params"] = parameters;
            return request;
        }
    }
}
