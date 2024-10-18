using Microsoft.AspNetCore.Mvc;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class LecturerController : Controller
    {

        [HttpPost]
        public IActionResult SubmitClaim(int HoursWorked, decimal HourlyRate, string AdditionalNotes, IFormFile SupportingDocuments)
        {
            if (SupportingDocuments != null &&  SupportingDocuments.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", SupportingDocuments.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    SupportingDocuments.CopyTo(stream);
                }
            }

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
