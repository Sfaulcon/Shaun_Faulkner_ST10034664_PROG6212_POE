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
        public IActionResult SubmitClaim(Claim claim, IFormFile SupportingDocument)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            var lecturerIdVal = HttpContext.Session.GetString("LecturerId");
            var lecturerName = HttpContext.Session.GetString("LecturerName");
            var lecturerEmail = HttpContext.Session.GetString("LecturerEmail");

            if (string.IsNullOrEmpty(lecturerIdVal) || string.IsNullOrEmpty(lecturerName) || string.IsNullOrEmpty(lecturerEmail))
            {
                ModelState.AddModelError("", "Lecturer information is missing from the session.");
                return View(claim);
            }

            claim.LecturerId = int.Parse(lecturerIdVal);
            claim.LecturerName = lecturerName;
            claim.LecturerEmail = lecturerEmail;

            claim.SubmissionDate = DateTime.Now;
            claim.HoursWorked = claim.HoursWorked;
            claim.HourlyRate = claim.HourlyRate;
            claim.AdditionalNotes = claim.AdditionalNotes;

            if (claim.HoursWorked > 0 && claim.HourlyRate > 0)
            {
                claim.Status = "Approved";
                claim.TotalAmount = (decimal)(claim.HoursWorked * claim.HourlyRate);
            }
            else
            {
                claim.Status = "Denied";
                claim.TotalAmount = 0;
            }

            if (SupportingDocument != null && SupportingDocument.Length > 0)
            {
                var uploadsFolder = @"C:\Users\shaun\source\repos\Shaun_Faulkner_ST10034664_PROG6212_POE\Shaun_Faulkner_ST10034664_PROG6212_POE\wwwroot\uploads\supportingdocs";

                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + SupportingDocument.FileName;

                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    SupportingDocument.CopyTo(fileStream);
                }

                claim.SupportingDocuments = "/uploads/" + uniqueFileName;
            }
            else
            {
                claim.SupportingDocuments = null;
            }

            _context.Claims.Add(claim);
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Lecturer");

        }
    }
}
