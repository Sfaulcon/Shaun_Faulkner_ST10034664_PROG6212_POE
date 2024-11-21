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

            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("LecturerId") == null)
            {
                return RedirectToAction("LecturerLogin");
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LecturerLogin");
        }
    }
}
