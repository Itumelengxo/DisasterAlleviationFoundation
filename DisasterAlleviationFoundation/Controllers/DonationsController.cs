using System.Linq;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class DonationsController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            var list = InMemoryStore.Donations.Values.OrderByDescending(d => d.CreatedAt).ToList();
            return View(list);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(string donorName, string resourceType, int quantity = 0, decimal? amount = null)
        {
            if (string.IsNullOrWhiteSpace(resourceType))
            {
                ViewBag.Error = "Resource type required.";
                return View();
            }

            var id = InMemoryStore.NextDonationId();
            var donation = new Donation
            {
                Id = id,
                DonorName = donorName,
                ResourceType = resourceType,
                Quantity = quantity,
                Amount = amount,
                CreatedAt = System.DateTime.UtcNow,
                CreatedBy = User.Identity?.Name
            };

            InMemoryStore.Donations[id] = donation;
            return RedirectToAction("Index");
        }
    }
}
