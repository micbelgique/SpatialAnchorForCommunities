using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApi.Views.Home
{
    public class Secret : PageModel
    {
        public string Message { get; private set; } = "PageModel in C#";

        public void OnGet()
        {
            Message += $" Server time is { DateTime.Now }";
        }
    }
}