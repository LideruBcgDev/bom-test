using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BomManagement.WEB.Controllers.Web
{
    [Authorize]
    public class MainMenuController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
} 