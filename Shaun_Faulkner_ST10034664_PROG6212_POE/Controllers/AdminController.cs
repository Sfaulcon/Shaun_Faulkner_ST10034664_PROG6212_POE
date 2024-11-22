using Microsoft.AspNetCore.Mvc;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Data;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Models;
using System.ComponentModel.DataAnnotations;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(string email, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminEmail", admin.Email);
                return RedirectToAction("AdminDashboard");
            }
            else
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }
        }

        public IActionResult AdminDashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminEmail")))
            {
                return RedirectToAction("AdminLogin");
            }

            var pendingClaims = _context.Claims.Where(c => c.Status == "Pending").ToList();
            return View(pendingClaims);
        }

        [HttpPost]
        public IActionResult ApproveDenyClaims(int claimId, string status)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction("AdminDashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("AdminLogin");
        }
    }

}
