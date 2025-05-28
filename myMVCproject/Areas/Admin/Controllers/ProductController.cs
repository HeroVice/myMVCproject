using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyMVCProject.DataAccess.Repository.IRepository;
using MyMVCProject.Models;
using MyMVCProject.Models.ViewModels;

namespace MyMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create product
                return View(productVM);
            }
            else
            {
                //update product
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "ProductImages");
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile? cover, List<IFormFile>? files)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                // CREATE
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    _unitOfWork.Save();
                }
                else
                {
                    // UPDATE
                    var productFromDb = _unitOfWork.Product.Get(p => p.Id == productVM.Product.Id);
                    if (productFromDb == null)
                    {
                        return NotFound();
                    }

                    productFromDb.Name = productVM.Product.Name;
                    productFromDb.Description = productVM.Product.Description;
                    productFromDb.Price = productVM.Product.Price;
                    productFromDb.PublishDate = productVM.Product.PublishDate;
                    productFromDb.Publisher = productVM.Product.Publisher;
                    productFromDb.CategoryId = productVM.Product.CategoryId;

                    _unitOfWork.Product.Update(productFromDb);
                    _unitOfWork.Save();

                    productVM.Product = productFromDb;
                }

                // COVER IMAGE
                if (cover != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(cover.FileName);
                    string productPath = Path.Combine("images", "products", "CoverProduct-" + productVM.Product.Id);
                    string finalPath = Path.Combine(wwwRootPath, productPath);

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }

                    string filePath = Path.Combine(finalPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        cover.CopyTo(stream);
                    }

                    productVM.Product.CoverImageUrl = Path.Combine("/", productPath, fileName).Replace("\\", "/");
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                }

                // GALLERY IMAGES
                if (files != null && files.Count > 0)
                {
                    foreach (var item in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        string productPath = Path.Combine("images", "products", "Product-" + productVM.Product.Id);
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        string filePath = Path.Combine(finalPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }

                        var productImage = new ProductImage
                        {
                            ProductId = productVM.Product.Id,
                            ImageUrl = Path.Combine("/", productPath, fileName).Replace("\\", "/")
                        };

                        _unitOfWork.ProductImage.Add(productImage);
                    }
                    _unitOfWork.Save();
                }

                TempData["success"] = "Product saved successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                TempData["error"] = "Bir hata oluştu.";
                return View(productVM);
            }
        }


        [HttpPost]
        public IActionResult DeleteCover(int productId)
        {
            var product = _unitOfWork.Product.Get(p => p.Id == productId);
            if (product == null || string.IsNullOrEmpty(product.CoverImageUrl))
            {
                return NotFound();
            }

            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.CoverImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            product.CoverImageUrl = null;
            _unitOfWork.Product.Update(product);
            _unitOfWork.Save();

            return RedirectToAction("Upsert", new { id = productId });
        }

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;   //Get Product ID from Image ID
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);           //Delete image record from DB
                    }
                }
                _unitOfWork.ProductImage.Remove(imageToBeDeleted);   //Delete image record from DB
                _unitOfWork.Save();   //Save changes to DB

                TempData["success"] = "Image deleted successfully.";
            }
            return RedirectToAction(nameof(Upsert), new { id = productId });
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }

            string productFolder = $"product-{id}";
            string productPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products", productFolder);

            if (Directory.Exists(productPath))
            {
                try
                {
                    // Dosyaları sil
                    var files = Directory.GetFiles(productPath);
                    foreach (var file in files)
                    {
                        System.IO.File.Delete(file);
                    }

                    // Klasörü sil
                    Directory.Delete(productPath);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Dosyalar silinirken hata oluştu: " + ex.Message });
                }
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Silme işlemi başarılı" });
        }
        #endregion
    }
}
