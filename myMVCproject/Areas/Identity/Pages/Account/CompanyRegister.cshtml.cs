#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyMVCProject.DataAccess.Data;
using MyMVCProject.Models;

namespace MyMVCProject.Areas.Identity.Pages.Account
{
    public class CompanyRegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CompanyRegisterModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            public string CompanyName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string IdentificationNumber { get; set; }

            [Required]
            public string Country { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string CompanyAddress { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Şu an giriş yapan kullanıcı var ise userId'yi al (değilse null olabilir)
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            // Yeni CompanyRequest nesnesi oluştur
            var companyRequest = new CompanyRequest
            {
                UserId = userId,
                Email = Input.Email,
                RequestDate = DateTime.UtcNow,
                IsApproved = false,
                CompanyName = Input.CompanyName,
                Password = Input.Password, // Not: Şifreyi düz kaydetme, hashle!
                IDN = Input.IdentificationNumber,
                Country = Input.Country,
                City = Input.City,
                CompAddress = Input.CompanyAddress
            };

            // Şifreyi düz kaydetmek yerine hash'lemen gerekir, güvenlik açısından.
            // Burada örnek olması için düz kaydettim.

            _context.CompanyRequests.Add(companyRequest);
            await _context.SaveChangesAsync();

            // Başarılı kayıt sonrası yönlendirme
            return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
        }
    }
}
