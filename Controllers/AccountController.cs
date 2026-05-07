using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using BarangaySystem.Data;
using BarangaySystem.Models;
using BarangaySystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BarngaySystem.Controllers
{
    public class AccountController : Controller
    {
        // _context lets us query the database
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show Login Page
        public IActionResult Login() => View();

        // Process Login Form
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //Step 1: Check if form is valid (required fields filled, valid email format)
            if (!ModelState.IsValid) return View(model);

            //Step 2: Find the user by email in the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            //Step 3: If not found, or password is wrong, show erroe
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Wrong email or password");
                return View(model);
            }

            //Step 4: Build the login 'claims' (info stored in the cookie)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), //User ID
                new Claim(ClaimTypes.Name, user.FullName),                //Display name
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)                     //User role (Admin, User)
            };

            //Step 5: Sign the user in (creates the cookie)
            var identity = new ClaimsIdentity(claims, "CookieAuth");
            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(identity));

            //Step 6: Redirect to the right page based on role
            return user.Role == "Admin"
                ? RedirectToAction("Dashboard", "Admin")
                : RedirectToAction("Index", "Home");
        }

        // SHOW Register Page
        public IActionResult Register() => View();

        // PROCESS Register Form
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            //Check if email is already taken
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            //Hash the password before saving (NEVER save plain text)
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Address = model.Address,
                ContactNumber = model.ContactNumber,
                Role = "Resident"  //Always register as Resident
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Register! You can now log in.";
            return RedirectToAction("Login");
        }
    }
}