using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NetJsonRpc.Pages
{
    public class MessageModel : PageModel
    {
        public string Message { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MessageCode { get; set; }

        public void OnGet()
        {
            if(MessageCode == "1")
            {
                Message = "Authentication failed";
            }
            else
            {
                Message = "No message available";
            }
        }
    }
}