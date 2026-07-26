using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SafeX.CompanyPanel.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        [HttpGet]
        public IActionResult Start(int internId)
        {
            ViewBag.InternId = internId;
            return View();
        }
    }
}
