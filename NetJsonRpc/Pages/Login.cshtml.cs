using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using NetJsonRpc.Auth;

namespace NetJsonRpc.Pages
{
    public class LoginModel : PageModel
    {
        public string Message { get; set; }
        public bool IsUserLogged { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        public void OnGet()
        {
            User user = HttpContext.Session.GetUser();

            Message = user != null ? user.Username + " logged" : "No logged user";

            IsUserLogged = user != null;
        }

        public IActionResult OnPost()
        {
            var username = HttpContext.Request.Form["username"];
            var password = HttpContext.Request.Form["password"];

            User user = LoginModule.Login(username, password);

            if(user == null)
            {
                // HTTP 302
                // Location /Message?MessageCode=1
                return RedirectToPage("/Message", new { MessageCode = "1" });
            }

            // See Auth.SessionExtensions
            HttpContext.Session.SetUser(user);

            Message = user.Username + " logged";

            IsUserLogged = true;

            if(Type != null && Type.Equals("json"))
            {
                return Content("{username=\"" + user.Username + "\"}", "application/json", System.Text.Encoding.UTF8);
            }

            return Page();
        }
    }
 }