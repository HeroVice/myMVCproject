using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MyMVCProject.DataAccess.Repository.IRepository;
using MyMVCProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MyMVCProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var cartItems = _unitOfWork.ShoppingCart
                .GetAll(sc => sc.UserId == user.Id, includeProperties: "Product")
                .ToList();

            foreach (var item in cartItems)
            {
                item.Price = item.Product.ListPrice;
            }

            return View(cartItems);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cartItem = _unitOfWork.ShoppingCart.Get(sc => sc.Id == id);
            if (cartItem != null)
            {
                _unitOfWork.ShoppingCart.Remove(cartItem);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int count = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var existingCart = _unitOfWork.ShoppingCart.Get(
                sc => sc.UserId == user.Id && sc.ProductId == productId
            );

            if (existingCart != null)
            {
                existingCart.Count += count;
                _unitOfWork.ShoppingCart.Update(existingCart);
            }
            else
            {
                var newCart = new ShoppingCart
                {
                    ProductId = productId,
                    UserId = user.Id,
                    Count = count
                };
                _unitOfWork.ShoppingCart.Add(newCart);
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
