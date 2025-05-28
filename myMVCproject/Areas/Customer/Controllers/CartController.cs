using Microsoft.AspNetCore.Mvc;

namespace MyMVCProject.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
