using System.Linq;
using System.Threading.Tasks;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class IncidentsController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            var list = InMemoryStore.Incidents.Values.OrderByDescending(i => i.ReportedAt).ToList();
            return View(list);
        }

        [Authorize]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Create(string type, string location, string description)
        {
            if (string.IsNullOrWhiteSpace(type) || string.IsNullOrWhiteSpace(location))
            {
                ViewBag.Error = "Type and location are required.";
                return View();
            }

            var id = InMemoryStore.NextIncidentId();
            var incident = new Incident
            {
                Id = id,
                Type = type,
                Location = location,
                Description = description,
                ReportedAt = System.DateTime.UtcNow,
                ReportedBy = User.Identity?.Name
            };

            InMemoryStore.Incidents[id] = incident;
            return RedirectToAction("Details", new { id });
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            if (!InMemoryStore.Incidents.TryGetValue(id, out var incident)) return NotFound();
            return View(incident);
        }
    }
}
