using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyMVCProject.DataAccess.Data;
using MyMVCProject.Models;

namespace MyMVCProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitRating(int productId, int rating, string content)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var existingReview = _context.Reviews
                .FirstOrDefault(r => r.ProductId == productId && r.UserId == userId);

            if (existingReview == null)
            {
                var newReview = new Review
                {
                    ProductId = productId,
                    UserId = userId,
                    Rating = rating,
                    Content = content,
                    CreatedAt = DateTime.Now
                };
                _context.Reviews.Add(newReview);
            }
            else
            {
                existingReview.Rating = rating;
                existingReview.Content = content;
                existingReview.CreatedAt = DateTime.Now;
            }

            _context.SaveChanges();
            return RedirectToAction("Details", "Home", new { area = "Customer", productId = productId });
        }
    }
}
