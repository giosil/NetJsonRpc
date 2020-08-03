using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetJsonRpc.Auth
{
    public class User
    {
        private string username;
        private string role;
        private string email;

        public User()
        {
        }

        public User(string username)
        {
            this.username = username;
        }

        public User(string username, string role, string email)
        {
            this.username = username;
            this.role = role;
            this.email = email;
        }

        public string Username { get => username; set => username = value; }
        public string Role { get => role; set => role = value; }
        public string Email { get => email; set => email = value; }
    }
}
