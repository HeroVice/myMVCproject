using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMVCProject.DataAccess.Data;

namespace MyMVCProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RequestController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Requests()
        {
            var pendingRequests = _context.CompanyRequests
                .Where(r => !r.IsApproved)
                .ToList();

            ViewBag.UserManager = _userManager;

            return View(pendingRequests);
        }


        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var request = _context.CompanyRequests.FirstOrDefault(r => r.Id == id);
            if (request == null) return NotFound();

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return NotFound();

            // Rolü ekle
            await _userManager.AddToRoleAsync(user, "Company");

            // Onaylandı olarak işaretle
            request.IsApproved = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Requests");
        }
    }
}
