using CustomerManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepo;
        public AccountController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult RegisterUser(string username, string password)
        {
            var newUser = _userRepo.RegisterUser(username, password);
            if (newUser != null)
            {
                ViewBag.Success = "Registration successful! Please login.";
                return RedirectToAction("Account","Login");
            }
            ViewBag.Error = "Username already exists.";
            return View("~/Views/Account/Register.cshtml");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult ValidateUser(string username, string password)
        {
            var user = _userRepo.ValidateUser(username, password);
            if (user != null)
            {
                HttpContext.Session.SetString("User", user.Username);
                return RedirectToAction("Customers", "Customers");
            }
            ViewBag.Error = "Invalid username or password";
            return View("~/Views/Account/Login.cshtml");
        }
    }
}
