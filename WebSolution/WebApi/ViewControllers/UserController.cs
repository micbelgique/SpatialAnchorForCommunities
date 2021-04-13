using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.User;

namespace WebApi.ViewControllers
{
    [Route("account")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login(LoginViewModel viewModel)
        {
            return Redirect("/Identity/Account/Login");
            //return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromForm] LoginViewModel model)
        {
            return RedirectToAction("Login");
        }
    }
}