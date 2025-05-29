using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyMVCProject.DataAccess.Repository.IRepository;
using MyMVCProject.Models;
using MyMVCProject.Utility;
using Microsoft.EntityFrameworkCore;
using MyMVCProject.Models.ViewModels;
using MyMVCProject.DataAccess.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

namespace MyMVCProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                try
                {
                    //user is logged in
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.UserId == claim.Value).Count());
                }
                catch
                {

                }
            }

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
            return View(productList);
        }

        public IActionResult Catalog(int productId,int? categoryId, string publisher, decimal? minPrice, decimal? maxPrice, string? sortOrder, string searchTerm)
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages").ToList();

            if (categoryId.HasValue && categoryId.Value > 0)
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();

            if (!string.IsNullOrEmpty(publisher))
                products = products.Where(p => p.Publisher.ToLower().Contains(publisher.ToLower())).ToList();

            if (minPrice.HasValue)
                products = products.Where(p => p.ListPrice >= minPrice.Value).ToList();

            if (maxPrice.HasValue)
                products = products.Where(p => p.ListPrice <= maxPrice.Value).ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                products = (List<Product>)products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }


            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.ListPrice).ToList();
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ListPrice).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
            }

            var categories = _unitOfWork.Category.GetAll().ToList();

            var vm = new ProductCatalogVM
            {
                Products = products,
                SelectedCategoryId = categoryId,
                Publisher = publisher,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortOrder = sortOrder,
                SearchTerm = searchTerm,
                Categories = categories
            };

            return View(vm);
        }


        public IActionResult Details(int productId)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;
            var product = _unitOfWork.Product.GetProductWithCategory(productId);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductShopVM
            {
                Product = product,
                ShoppingCart = new ShoppingCart(),
                IsInBasket = false,
                IsInLibrary = false,
                ExistingReview = _unitOfWork.Review.Get(r => r.ProductId == productId && r.UserId == userId)
            };


            if (claim != null)
            {
                var basketEntry = _unitOfWork.ShoppingCart.Get(x => x.ProductId == productId && x.UserId == userId);

                if (basketEntry != null)
                {
                    viewModel.IsInBasket = true;
                }

                var libraryEntry = _unitOfWork.Library.Get(x => x.ProductId == productId && x.UserId == userId);

                if (libraryEntry != null)
                {
                    viewModel.IsInLibrary = true;
                }
            }

            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
