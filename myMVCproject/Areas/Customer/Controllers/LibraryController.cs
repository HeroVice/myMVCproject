using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMVCProject.DataAccess.Repository.IRepository;
using MyMVCProject.Models;
using MyMVCProject.Utility;

namespace MyMVCProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class LibraryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public LibraryController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var libraryItems = _unitOfWork.Library.GetAll(l => l.UserId == userId, includeProperties: "Product");
            return View(libraryItems);
        }
    }
}
