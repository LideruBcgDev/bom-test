using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BomManagement.WEB.Controllers
{
    [Authorize]
    public class MainMenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 