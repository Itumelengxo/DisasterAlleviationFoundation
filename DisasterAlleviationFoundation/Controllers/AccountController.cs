using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Helpers;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundation.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(string username, string password, string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Username and password are required.";
                return View();
            }

            if (InMemoryStore.Users.ContainsKey(username))
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            var (hash, salt) = PasswordHelper.HashPassword(password);
            var user = new UserModel
            {
                Username = username,
                PasswordHash = hash,
                PasswordSalt = salt,
                FullName = fullName,
                Email = email,
                Role = "User"
            };

            InMemoryStore.Users[username] = user;

            return RedirectToAction("Login");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (!InMemoryStore.Users.TryGetValue(username, out var user))
            {
                ViewBag.Error = "Invalid credentials.";
                return View();
            }

            var ok = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
            if (!ok)
            {
                ViewBag.Error = "Invalid credentials.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FullName", user.FullName ?? string.Empty),
                new Claim("Email", user.Email ?? string.Empty)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: /Account/Profile
        public IActionResult Profile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username) || !InMemoryStore.Users.TryGetValue(username, out var user))
                return RedirectToAction("Login");

            return View(user);
        }

        // POST: /Account/Profile
        [HttpPost]
        public IActionResult Profile(string fullName, string email, string phone)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username) || !InMemoryStore.Users.TryGetValue(username, out var user))
                return RedirectToAction("Login");

            user.FullName = fullName;
            user.Email = email;
            user.Phone = phone;

            InMemoryStore.Users[username] = user;
            ViewBag.Message = "Profile updated (in-memory only).";
            return View(user);
        }
    }
}
