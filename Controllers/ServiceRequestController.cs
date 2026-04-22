using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class ServiceRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
