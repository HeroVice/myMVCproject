using Microsoft.AspNetCore.Mvc;

namespace MyMVCProject.Areas.Company.Controllers
{
    [Area("Company")]
    public class ManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}
