using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyMVCProject.DataAccess.Data;
using MyMVCProject.Models;
using MyMVCProject.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using MyMVCProject.Models.ViewModels;

namespace MyMVCProject.Areas.Customer.Controllers
{

    [Area("Customer")]
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public WishlistController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Wishlist()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var wishlistItems = _unitOfWork.Wishlist
                .GetAll(w => w.UserId == userId, includeProperties: "Product")
                .ToList();

            var wishlistVMs = wishlistItems.Select(w => new ProductShopVM
            {
                Product = w.Product
            }).ToList();

            return View(wishlistVMs);
        }


        [HttpPost]
        public IActionResult AddWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var newWishlist = new Wishlist
            {
                ProductId = productId,
                UserId = userId,
                AddedAt = DateTime.Now
            };
            _unitOfWork.Wishlist.Add(newWishlist);
            _unitOfWork.Save();

            return RedirectToAction("Details", "Home", new { area = "Customer", productId = productId });
        }

        [HttpPost]
        public IActionResult RemoveWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var wishlistItem = _unitOfWork.Wishlist.Get(w => w.ProductId == productId && w.UserId == userId);
            if (wishlistItem != null)
            {
                _unitOfWork.Wishlist.Remove(wishlistItem);
                _unitOfWork.Save();
            }
            return RedirectToAction("Details", "Home", new { area = "Customer", productId = productId });
        }
    }
}
