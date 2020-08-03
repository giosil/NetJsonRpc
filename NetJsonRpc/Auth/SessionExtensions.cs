using System;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetJsonRpc.Auth
{
    public static class SessionExtensions
    {
        public static void SetUser(this ISession session, User user)
        {
            if(user == null)
            {
                session.Remove("user");

                return;
            }

            string jsonUser = JsonConvert.SerializeObject(user);

            session.SetString("user", jsonUser);
        }

        public static User GetUser(this ISession session)
        {
            var value = session.GetString("user");

            if(value == null || value.Length < 3)
            {
                return null;
            }

            JObject jobject = JObject.Parse(value);

            return jobject.ToObject<User>();
        }
    }
}
