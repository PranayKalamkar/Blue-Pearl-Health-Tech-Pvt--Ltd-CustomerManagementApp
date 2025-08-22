using CustomerManagementApp.Models;
using CustomerManagementApp.Repositories;
using CustomerManagementApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementApp.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IEmailService _emailService;

        public CustomersController(ICustomerRepository customerRepo, IEmailService emailService)
        {
            _customerRepo = customerRepo;
            _emailService = emailService;
        }

        public IActionResult Customers()
        {
            var customers = _customerRepo.GetAllCustomers();
            return View(customers);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            if (_customerRepo.AddCustomer(customer, out string errorMessage))
            {
                TempData["Success"] = "Customer added successfully.";
                return RedirectToAction("Customers", "Customers"); // Your list page
            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(customer);
            }
        }


        public IActionResult Email(int id)
        {
            var customer = _customerRepo.GetAllCustomers().FirstOrDefault(c => c.CustomerId == id);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(int id, string subject, string body, IFormFile attachment)
        {
            var customer = _customerRepo.GetAllCustomers().FirstOrDefault(c => c.CustomerId == id);
            if (customer == null)
            {
                TempData["Error"] = "Customer not found.";
                return RedirectToAction("Customers", "Customers");
            }

            bool result = await _emailService.SendEmailAsync(customer.Email, subject, body, attachment);

            if (result)
                TempData["Success"] = "Email sent successfully.";
            else
                TempData["Error"] = "Failed to send email. Please check SMTP settings.";

            return RedirectToAction("Customers", "Customers");
        }
    }
}
