using Microsoft.AspNetCore.Mvc;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Data;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Models;
using Microsoft.AspNetCore.Http;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly AppDbContext _context;

        public LecturerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult LecturerSignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LecturerSignUp(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Add(lecturer);
                _context.SaveChanges();

                return RedirectToAction("LecturerLogin");
            }

            return View(lecturer);
        }

        [HttpGet]
        public IActionResult LecturerLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LecturerLogin(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Both fields are required.");
                return View();
            }

            var lecturer = _context.Lecturers.SingleOrDefault(l => l.Email == email && l.Password == password);

            if (lecturer == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password.");
                return View();
            }

            HttpContext.Session.SetString("LecturerId", lecturer.LecturerId.ToString());
            HttpContext.Session.SetString("LecturerName", lecturer.Name);
            HttpContext.Session.SetString("LecturerEmail", lecturer.Email);

            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("LecturerId") == null)
            {
                return RedirectToAction("LecturerLogin");
            }

            var lecturerId = int.Parse(HttpContext.Session.GetString("LecturerId"));

            var pendingClaims = _context.Claims.Where(c => c.LecturerId == lecturerId && c.Status == "Pending").ToList();
            var approvedClaims = _context.Claims.Where(c => c.LecturerId == lecturerId && c.Status == "Approved").ToList();
            var deniedClaims = _context.Claims.Where(c => c.LecturerId == lecturerId && c.Status == "Denied").ToList();

            foreach (var claim in approvedClaims)
            {
                if (claim.Status == "Approved")
                {
                    var invoicePath = Path.Combine("/invoices", $"Invoice_{claim.ClaimId}.pdf");
                    claim.InvoicePath = invoicePath;
                }
            }

            var model = new ClaimsDashboardViewModel
            {
                PendingClaims = pendingClaims,
                ApprovedClaims = approvedClaims,
                DeniedClaims = deniedClaims
            };

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LecturerLogin");
        }

        public IActionResult ManageProfile()
        {
            var lecturerId = int.Parse(HttpContext.Session.GetString("LecturerId"));

            var lecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == lecturerId);

            if (lecturer == null)
            {
                return View("LecturerLogin");
            }

            return View(lecturer);
        }

        [HttpPost]
        public IActionResult UpdateProfile(Lecturer lecturer)
        {
            var lecturerId = int.Parse(HttpContext.Session.GetString("LecturerId"));

            var existingLecturer = _context.Lecturers.FirstOrDefault(l => l.LecturerId == lecturerId);

            if (existingLecturer == null)
            {
                return RedirectToAction("LecturerLogin");
            }

            existingLecturer.Name = lecturer.Name;
            existingLecturer.Surname = lecturer.Surname;
            existingLecturer.Email = lecturer.Email;

            if (!string.IsNullOrEmpty(lecturer.Password))
            {
                existingLecturer.Password = lecturer.Password;
            }

            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}
