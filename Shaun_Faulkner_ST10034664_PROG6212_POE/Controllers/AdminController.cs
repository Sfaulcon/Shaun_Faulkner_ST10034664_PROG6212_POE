using Microsoft.AspNetCore.Mvc;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Models;
using System.ComponentModel.DataAnnotations;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult ApproveDenyClaims()
        {
            var pendingClaims = _context.Claims.Where(c => c.Status == "Pending").ToList();

            return View(pendingClaims);
        }

        [HttpPost]
        public ActionResult ApproveClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                _context.SaveChanges();
            }

            return RedirectToAction("ApproveDenyClaims");
        }

        [HttpPost]
        public ActionResult DenyClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                claim.Status = "Denied";
                _context.SaveChanges();
            }

            return RedirectToAction("ApproveDenyClaims");
        }
    }

}
