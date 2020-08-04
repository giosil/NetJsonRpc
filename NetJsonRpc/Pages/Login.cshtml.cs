using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NetJsonRpc.Auth;

namespace NetJsonRpc.Pages
{
    public class LoginModel : PageModel
    {
        public string Message { get; set; }
        public bool IsUserLogged { get; set; }

        public void OnGet()
        {
            User user = HttpContext.Session.GetUser();

            Message = user != null ? user.Username + " logged" : "No logged user";

            IsUserLogged = user != null;
        }

        public void OnPost()
        {
            var username = HttpContext.Request.Form["username"];
            var password = HttpContext.Request.Form["password"];

            User user = LoginModule.Login(username, password);

            // See Auth.SessionExtensions
            HttpContext.Session.SetUser(user);

            Message = user != null ? user.Username + " logged" : "No logged user";

            IsUserLogged = user != null;
        }
    }
 }