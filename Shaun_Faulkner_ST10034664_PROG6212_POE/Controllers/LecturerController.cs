using Microsoft.AspNetCore.Mvc;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Models;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class LecturerController : Controller
    {

        private readonly ApplicationDbContext _context;

        public LecturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult SubmitClaim()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile SupportingDocuments)
        {
            if (SupportingDocuments != null && SupportingDocuments.Length > 0)
            {
                if (SupportingDocuments.Length > 2 * 1024 * 1024 ||
                    !(Path.GetExtension(SupportingDocuments.FileName).ToLower() == ".pdf" ||
                    Path.GetExtension(SupportingDocuments.FileName).ToLower() == ".docx" ||
                    Path.GetExtension(SupportingDocuments.FileName).ToLower() == ".xlsx"))
                {
                    ModelState.AddModelError("File", "File Size or type is invalid");
                    return View(claim);
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", SupportingDocuments.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await SupportingDocuments.CopyToAsync(stream);
                }

                claim.SupportingDocumentsPath = "/uploads/" + SupportingDocuments.FileName;

                claim.Status = "Pending";
                claim.SubmissionDate = DateTime.Now;

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                return RedirectToAction("Dashboard");
            }

            return View(claim);
        }

        public ActionResult Dashboard()
        {
            var lecturerClaims = _context.Claims.ToList();
            return View(lecturerClaims);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Add(lecturer);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Lecturer");
            }

            return View(lecturer);
        }
    }
}
