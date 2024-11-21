using Microsoft.AspNetCore.Mvc;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Data;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Models;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class ClaimController : Controller
    {
        private readonly AppDbContext _context;

        public ClaimController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitClaim(Claim claim, IFormFile supportingDocument)
        {
            if (ModelState.IsValid)
            {
                if (supportingDocument != null && supportingDocument.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", supportingDocument.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        supportingDocument.CopyTo(stream);
                    }

                    claim.SupportingDocuments = "/uploads/" + supportingDocument.FileName;
                }

                claim.LecturerId = int.Parse(HttpContext.Session.GetString("LecturerId"));
                claim.LecturerName = HttpContext.Session.GetString("LecturerName");
                claim.LecturerEmail = HttpContext.Session.GetString("LecturerEmail");
                claim.Status = "Pending";
                claim.SubmissionDate = DateTime.Now;

                _context.Claims.Add(claim);
                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Lecturer");
            }

            return View(claim);
        }
    }
}
