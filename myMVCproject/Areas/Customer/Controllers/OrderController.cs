using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMVCProject.DataAccess.Repository.IRepository;
using MyMVCProject.Models;

namespace MyMVCProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = _unitOfWork.ShoppingCart.GetAll(c => c.UserId == userId, includeProperties: "Product");

            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.Total = cartItems.Sum(c => c.Price * c.Count);
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckoutPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = _unitOfWork.ShoppingCart.GetAll(
                c => c.UserId == userId,
                includeProperties: "Product"
            ).ToList();

            if (!cartItems.Any())
            {
                TempData["error"] = "Sepetiniz boş!";
                return RedirectToAction("Index", "Cart");
            }

            foreach (var item in cartItems)
            {
                if (item.ProductId == 0 || item.Product == null)
                    continue;

                // OrderDetail tablosuna kayıt
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    Price = item.Product.Price,
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = "Onaylandı"
                };
                _unitOfWork.OrderDetail.Add(orderDetail);

                // Library tablosuna kayıt
                var libraryItem = new Library
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ProductId = item.ProductId,
                    DateAdded = DateTime.Now,
                    IsPurchased = true
                };
                _unitOfWork.Library.Add(libraryItem);
            }

            // Sepeti temizle
            _unitOfWork.ShoppingCart.Remove(cartItems);

            // Değişiklikleri kaydet
            _unitOfWork.Save();

            TempData["success"] = "Ödeme tamamlandı. Ürünler kütüphaneye eklendi.";
            return RedirectToAction("Index", "Library");
        }




        public IActionResult Success()
        {
            return View(); // Basit bir "Sipariş başarıyla oluşturuldu" sayfası
        }
    }
}
