using Microsoft.AspNetCore.Mvc;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Data;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using Document = iTextSharp.text.Document;
using System.Reflection.Metadata;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        // Filepath directory
        private readonly string _invoiceStoragePath = @"C:\Users\shaun\source\repos\Shaun_Faulkner_ST10034664_PROG6212_POE\Shaun_Faulkner_ST10034664_PROG6212_POE\wwwroot\uploads\invoices";

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(string email, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminEmail", admin.Email);
                return RedirectToAction("AdminDashboard");
            }
            else
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }
        }

        public IActionResult AdminDashboard()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AdminEmail")))
            {
                return RedirectToAction("AdminLogin");
            }

            var model = new ClaimsDashboardViewModel
            {
                PendingClaims = _context.Claims.Where(c => c.Status == "Pending").ToList(),
                ApprovedClaims = _context.Claims.Where(c => c.Status == "Approved").ToList(),
                DeniedClaims = _context.Claims.Where(c => c.Status == "Denied").ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ApproveDenyClaims(int claimId, string status)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.Status = status;
                _context.SaveChanges();
            }

            return RedirectToAction("AdminDashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("AdminLogin");
        }

        public IActionResult GenerateInvoice(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);

            if (claim == null || claim.Status != "Approved")
            {
                return NotFound();
            }

            string basePath = @"C:\Users\shaun\source\repos\Shaun_Faulkner_ST10034664_PROG6212_POE\Shaun_Faulkner_ST10034664_PROG6212_POE\wwwroot\uploads\invoices";
            var invoicePath = Path.Combine(basePath, $"Invoice_{claim.ClaimId}.pdf");

            if (!System.IO.File.Exists(invoicePath))
            {
                return NotFound($"File not found: {invoicePath}");
            }

            return File(invoicePath, "application/pdf", $"Invoice_{claimId}.pdf");
        }

        public IActionResult GenerateAllApprovedInvoices()
        {
            var approvedClaims = _context.Claims.Where(c => c.Status == "Approved").ToList();

            if (!approvedClaims.Any())
            {
                return NotFound();
            }

            string basePath = @"C:\Users\shaun\source\repos\Shaun_Faulkner_ST10034664_PROG6212_POE\Shaun_Faulkner_ST10034664_PROG6212_POE\wwwroot\uploads\invoices";
            var summaryInvoicePath = Path.Combine(basePath, "ApprovedClaimsSummaryInvoice.pdf");

            var summaryInvoice = CreateSummaryInvoice(approvedClaims);

            if (!System.IO.File.Exists(summaryInvoicePath))
            {
                return NotFound($"File not found: {summaryInvoicePath}");
            }

            return File(summaryInvoicePath, "application/pdf", "ApprovedClaimsSummaryInvoice.pdf");
        }

        private string CreateInvoice(Claim claim)
        {
            var filePath = Path.Combine(_invoiceStoragePath, $"Invoice_{claim.ClaimId}.pdf");

            if (!Directory.Exists(_invoiceStoragePath))
            {
                Directory.CreateDirectory(_invoiceStoragePath);
            }

            var document = new Document();

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                var writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                document.Add(new Paragraph("Invoice"));
                document.Add(new Paragraph($"Lecturer Name: {claim.LecturerName}"));
                document.Add(new Paragraph($"Lecturer Email: {claim.LecturerEmail}"));
                document.Add(new Paragraph($"Hours Worked: {claim.HoursWorked}"));
                document.Add(new Paragraph($"Hourly Rate: R {claim.HourlyRate}"));
                document.Add(new Paragraph($"Additional Notes: {claim.AdditionalNotes}"));
                document.Add(new Paragraph($"Total Amount Payable: R {claim.TotalAmount}"));

                document.Close();
            }
            return filePath;
        }

        private string CreateSummaryInvoice(List<Claim> approvedClaims)
        {
            var filePath = Path.Combine(_invoiceStoragePath, "ApprovedClaimsSummaryInvoice.pdf");

            if (!Directory.Exists(_invoiceStoragePath))
            {
                Directory.CreateDirectory(_invoiceStoragePath);
            }

            var document = new Document();

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                var writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                document.Add(new Paragraph("Approved Claims Summary Invoice"));

                foreach (var claim in approvedClaims)
                {
                    document.Add(new Paragraph($"Lecturer: {claim.LecturerName}, Total Amount: R {claim.TotalAmount}"));
                }

                document.Close();
            }

            return filePath;
        }
    }

}
