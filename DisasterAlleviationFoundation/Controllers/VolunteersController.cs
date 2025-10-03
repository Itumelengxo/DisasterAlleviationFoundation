using System.Linq;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundation.Controllers
{
    [Authorize]
    public class VolunteersController : Controller
    {
        // Browse volunteers
        [AllowAnonymous]
        public IActionResult Index()
        {
            var list = InMemoryStore.Volunteers.Values.ToList();
            return View(list);
        }

        // Register as volunteer (linked to logged in user)
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string skills, string availability)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Account");

            // Prevent duplicate volunteer for same username
            if (InMemoryStore.Volunteers.Values.Any(v => v.Username == username))
            {
                ViewBag.Message = "You are already registered as a volunteer.";
                return View();
            }

            var id = InMemoryStore.NextVolunteerId();
            var vol = new Volunteer
            {
                Id = id,
                Username = username,
                Skills = skills,
                Availability = availability,
                HoursContributed = 0
            };

            InMemoryStore.Volunteers[id] = vol;
            return RedirectToAction("MyProfile");
        }

        // My profile
        public IActionResult MyProfile()
        {
            var username = User.Identity?.Name;
            var vol = InMemoryStore.Volunteers.Values.FirstOrDefault(v => v.Username == username);
            if (vol == null) return RedirectToAction("Register");
            return View(vol);
        }

        // Tasks list
        [AllowAnonymous]
        public IActionResult Tasks()
        {
            var list = InMemoryStore.VolunteerTasks.Values.OrderBy(t => t.Start).ToList();
            return View(list);
        }

        // Accept a task (assign to current user)
        [HttpPost]
        public IActionResult AcceptTask(int taskId)
        {
            if (!InMemoryStore.VolunteerTasks.TryGetValue(taskId, out var task)) return NotFound();
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account");
            task.AssignedTo = username;
            InMemoryStore.VolunteerTasks[taskId] = task;

            // add to volunteer record if exists
            var vol = InMemoryStore.Volunteers.Values.FirstOrDefault(v => v.Username == username);
            if (vol != null)
            {
                vol.AssignedTaskIds.Add(taskId);
                InMemoryStore.Volunteers[vol.Id] = vol;
            }

            return RedirectToAction("Tasks");
        }

        // Simple endpoint to mark hours contributed
        [HttpPost]
        public IActionResult LogHours(int hours)
        {
            var username = User.Identity?.Name;
            var vol = InMemoryStore.Volunteers.Values.FirstOrDefault(v => v.Username == username);
            if (vol == null) return RedirectToAction("Register");
            vol.HoursContributed += hours;
            InMemoryStore.Volunteers[vol.Id] = vol;
            return RedirectToAction("MyProfile");
        }
    }
}
