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
                    ModelState.AddModelError("File", "File size must be less than 2MB and must be a .pdf, .docx, or .xlsx file.");
                    return View(claim);
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", SupportingDocuments.FileName);
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await SupportingDocuments.CopyToAsync(stream);
                    }
                }
                catch (IOException ex)
                {
                    ModelState.AddModelError("File", "There was an error uploading the file: " + ex.Message);
                    return View(claim);
                }

                claim.SupportingDocumentsPath = "/uploads/" + SupportingDocuments.FileName;
                claim.Status = "Pending";
                claim.SubmissionDate = DateTime.Now;

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("File", "Please select a file to upload.");
            return View(claim);
        }

        public IActionResult Dashboard()
        {
            var claims = _context.Claims.ToList();
            return View(claims);
        }
    }
}
