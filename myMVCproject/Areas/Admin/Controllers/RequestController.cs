using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMVCProject.DataAccess.Data;
using MyMVCProject.Models;
using System.Threading.Tasks;

[Area("Admin")]
public class RequestController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public RequestController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
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
        var request = await _context.CompanyRequests.FindAsync(id);
        if (request == null)
            return NotFound();

        // DOĞRUDAN ApplicationUser kullan
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            City = request.City,
            Country = request.Country,
            Address = request.CompAddress
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            TempData["Error"] = "Kullanıcı oluşturulamadı: " + string.Join(", ", createResult.Errors.Select(e => e.Description));
            return RedirectToAction("Requests");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Company");

        if (!roleResult.Succeeded)
        {
            TempData["Error"] = "Rol atanamadı: " + string.Join(", ", roleResult.Errors.Select(e => e.Description));
            return RedirectToAction("Requests");
        }

        // Yeni Company oluştur
        var company = new Company
        {
            UserId = user.Id,
            CompanyName = request.CompanyName
        };

        _context.Companies.Add(company);

        // Başvuru kaydını sil
        _context.CompanyRequests.Remove(request);

        await _context.SaveChangesAsync();

        TempData["Success"] = "Kullanıcı ve şirket başarıyla oluşturuldu, başvuru onaylandı.";
        return RedirectToAction("Requests");
    }

    [HttpPost]
    public async Task<IActionResult> DisapproveRequest(int id)
    {
        var request = await _context.CompanyRequests.FindAsync(id);
        if (request == null)
        {
            return NotFound();
        }

        _context.CompanyRequests.Remove(request);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Başvuru reddedildi ve veritabanından silindi.";
        return RedirectToAction("Requests");
    }

    // Index metodu veya listeyi dönen action da olmalı (isteğe göre)
}
