using Microsoft.AspNetCore.Mvc;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Services;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly TableStorageService _tableStorageService;

        public LecturerController()
        {
            _blobStorageService = new BlobStorageService("<blob connection string>", "<container name>");
            _tableStorageService = new TableStorageService("<table connection string>", "<table name>");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(IFormFile SupportingDocuments, string Name, string Surname, string Email, int HoursWorked)
        {
            string documentUrl = null;

            if (SupportingDocuments != null && SupportingDocuments.Length > 0)
            {
                using (var stream = SupportingDocuments.OpenReadStream())
                {
                    documentUrl = await _blobStorageService.UploadFileAsync(stream, SupportingDocuments.FileName);
                }
            }

            await _tableStorageService.SaveClaimAsync(Name, Surname, Email, HoursWorked, documentUrl);
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }
    }
}
