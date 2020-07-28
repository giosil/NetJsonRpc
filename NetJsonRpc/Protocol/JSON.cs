using Newtonsoft.Json;

namespace NetJsonRpc.Protocol
{
    public static class JSON
    {
        public static string Stringify(object obj)
        {
            if (obj == null) return "null";

            return JsonConvert.SerializeObject(obj);
        }

        public static object Parse(string text)
        {
            if(text == null || text.Length == 0)
            {
                return null;
            }

            return JsonConvert.DeserializeObject(text);
        }
    }
}
