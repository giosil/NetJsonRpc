using System;

namespace NetJsonRpc.Auth
{
    public static class LoginModule
    {
        public static User Login(string username, string password)
        {
            if (username == null || username.Length == 0)
            {
                return null;
            }
            if (password == null || password.Length == 0)
            {
                return null;
            }
            if(!username.Equals(password))
            {
                return null;
            }

            return new User(username, "oper", username + "@mail.com");
        }
    }
}
