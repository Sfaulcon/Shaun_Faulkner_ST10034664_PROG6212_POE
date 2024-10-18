using Microsoft.AspNetCore.Mvc;
using Shaun_Faulkner_ST10034664_PROG6212_POE.Models;

namespace Shaun_Faulkner_ST10034664_PROG6212_POE.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch current claims (e.g., those that are pending)
            var currentClaims = _context.Claims.Where(c => c.Status == "Pending").ToList();

            // Fetch past claims (e.g., those that are approved or denied)
            var pastClaims = _context.Claims.Where(c => c.Status == "Approved" || c.Status == "Denied").ToList();

            // Create a view model to hold both current and past claims
            var dashboardViewModel = new DashboardViewModel
            {
                CurrentClaims = currentClaims,
                PastClaims = pastClaims
            };

            return View(dashboardViewModel);
        }
    }
}
