using System.Collections.Generic;

namespace NetJsonRpc.Protocol
{
    public class RPCResponse
    {
        private int id;
        private bool success;
        private object result;
        private int errorCode;
        private string errorMessage;

        public RPCResponse(int id, object result)
        {
            this.id = id;
            this.result = result;
            this.success = true;
        }

        public RPCResponse(int id, int errorCode, string errorMessage)
        {
            this.id = id;
            this.ErrorCode = errorCode;
            this.errorMessage = errorMessage;
            this.success = false;
        }

        public int Id { get => id; set => id = value; }
        public bool Success { get => success; set => success = value; }
        public object Result { get => result; set => result = value; }
        public int ErrorCode { get => errorCode; set => errorCode = value; }
        public string ErrorMessage { get => errorMessage; set => errorMessage = value; }

        public Dictionary<string, object> GetDictionary()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            response["jsonrpc"] = "2.0";
            response["id"] = id;
            if (success)
            {
                response["result"] = result;
            }
            else
            {
                if(errorCode == 0)
                {
                    errorCode  = - 32000;
                }
                if(errorMessage == null || errorMessage.Length == 0)
                {
                    errorMessage = "Internal error";
                }

                IDictionary<string, object> error = new Dictionary<string, object>(2);
                error["code"] = errorCode;
                error["message"] = errorMessage;

                response["error"] = error;
            }
            return response;
        }
    }
}
