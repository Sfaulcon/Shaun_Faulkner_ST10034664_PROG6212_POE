using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class AdminController : Controller
    {

        private static List<Claim> _claims = new List<Claim>
        {
            new Claim { Id = 1, LecturerName = "Shaun Faulkner", Email = "shaunf@gmail.com", HoursWorked = 12, HourlyRate = 15.00m, AdditionalNotes = "Extra notes", SupportingDocumentUrl = "#", Status = "Pending"}
        };
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ApproveDenyClaims()
        {
            return View(new ClaimViewModel { Claims = _claims.Where(c => c.Status == "Pending").ToList() });
        }

        [HttpPost]
        public IActionResult ApproveDenyClaim(int claimId, string action)
        {
            var claim = _claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                if (action == "approve")
                {
                    claim.Status = "Approved";
                }
                else if (action == "deny")
                {
                    claim.Status = "Denied";
                }
            }

            return RedirectToAction("ApproveDenyClaims");
        }
    }

    public class Claim
    {
        public int Id { get; set; }
        public string LecturerName { get; set; }
        public string Email { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string AdditionalNotes { get; set; }
        public string SupportingDocumentUrl { get; set; }
        public string Status { get; set; }
    }

    public class ClaimViewModel
    {
        public List<Claim> Claims { get; set;}
    }

}
