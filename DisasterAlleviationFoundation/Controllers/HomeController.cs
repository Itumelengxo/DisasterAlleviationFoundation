using Microsoft.AspNetCore.Mvc;
using DisasterAlleviationFoundation.Models;
using System.Collections.Generic;

namespace DisasterAlleviationFoundation.Controllers
{
    public class HomeController : Controller
    {
        // Temporary in-memory collections
        public static List<Incident> Incidents = new();
        public static List<Donation> Donations = new();
        public static List<Volunteer> Volunteers = new();
        public static List<VolunteerTask> Tasks = new();

        public IActionResult Index()
        {
            ViewBag.Incidents = Incidents.Count;
            ViewBag.Donations = Donations.Count;
            ViewBag.Volunteers = Volunteers.Count;
            ViewBag.Tasks = Tasks.Count;

            return View();
        }
    }
}
