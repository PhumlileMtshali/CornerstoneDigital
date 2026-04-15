using CornerstoneDigital.Data;
using CornerstoneDigital.Models;
using CornerstoneDigital.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CornerstoneDigital.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public HomeController(IEmailService emailService, ApplicationDbContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public IActionResult Services()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Terms()
        {
            return View();
        }

        [Authorize]
        public IActionResult ProjectDetails(string id)
        {
            return View();
        }

        [Authorize]
        public IActionResult Quote()
        {
            return View();
        }

        [Authorize]
        public IActionResult Portfolio()
        {
            return View();
        }

        [Authorize]
        public IActionResult Pricing()
        {
            return View();
        }

        [Authorize]
        public IActionResult Testimonials()
        {
            return View();
        }

        [Authorize]
        public IActionResult PackageDetails(string service, string package)
        {
            return View();
        }

        [Authorize]
        public IActionResult ConfirmPackage(string service, string package)
        {
            ViewBag.Service = service;
            ViewBag.Package = package;
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Contact(string name, string email, string message)
        {
            ViewBag.Message = "Thank you for contacting us!";
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SubmitConfirmation(string service, string package, string companyName,
            string contactName, string email, string phone, string projectDetails)
        {
            ViewBag.Service = service;
            ViewBag.Package = package;
            ViewBag.CompanyName = companyName;
            ViewBag.ContactName = contactName;
            ViewBag.Email = email;
            ViewBag.Phone = phone;
            ViewBag.ProjectDetails = projectDetails;

            return View("Checkout");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(string service, string package, decimal amount,
            string companyName, string email, string paymentMethod)
        {
            var orderReference = $"ORD-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
            string packageName = GetPackageName(service, package);

            // SAVE ORDER TO DATABASE
            var order = new Order
            {
                OrderReference = orderReference,
                CompanyName = companyName,
                ContactName = ViewBag.ContactName ?? companyName,
                Email = email,
                Phone = ViewBag.Phone ?? "N/A",
                ServiceType = service,
                PackageName = packageName,
                Amount = amount,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Pending",
                ProjectDetails = ViewBag.ProjectDetails ?? "",
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["OrderReference"] = orderReference;

            switch (paymentMethod)
            {
                case "payfast":
                    return await RedirectToPayFastGateway(order, service, package, amount, email,
                        companyName, orderReference, packageName);

                case "stripe":
                    return await RedirectToStripeCheckout(order, service, package, amount, email,
                        companyName, orderReference, packageName);

                case "paypal":
                    return await RedirectToPayPal(order, service, package, amount, email,
                        companyName, orderReference, packageName);

                case "yoco":
                    return await RedirectToYoco(order, service, package, amount, email,
                        companyName, orderReference, packageName);

                case "eft":
                case "wire":
                    TempData["PaymentMethod"] = paymentMethod;
                    TempData["Amount"] = amount;
                    TempData["Reference"] = orderReference;
                    TempData["CompanyName"] = companyName;
                    TempData["Email"] = email;
                    TempData["PackageName"] = packageName;
                    return RedirectToAction("BankTransferDetails");

                default:
                    TempData["ErrorMessage"] = "Please select a payment method";
                    return RedirectToAction("Checkout");
            }
        }

        private async Task<IActionResult> RedirectToPayFastGateway(Order order, string service,
            string package, decimal amount, string email, string companyName,
            string orderReference, string packageName)
        {
            try
            {
                await _emailService.SendOrderConfirmationEmail(email, companyName, orderReference,
                    packageName, amount);

                // Mark as paid for demo purposes
                order.PaymentStatus = "Paid";
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            // TODO: Implement PayFast integration
            TempData["SuccessMessage"] = "Payment processed successfully!";
            return RedirectToAction("PaymentSuccess");
        }

        private async Task<IActionResult> RedirectToStripeCheckout(Order order, string service,
            string package, decimal amount, string email, string companyName,
            string orderReference, string packageName)
        {
            try
            {
                await _emailService.SendOrderConfirmationEmail(email, companyName, orderReference,
                    packageName, amount);

                // Mark as paid for demo purposes
                order.PaymentStatus = "Paid";
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            // TODO: Implement Stripe integration
            TempData["SuccessMessage"] = "Payment processed successfully!";
            return RedirectToAction("PaymentSuccess");
        }

        private async Task<IActionResult> RedirectToPayPal(Order order, string service,
            string package, decimal amount, string email, string companyName,
            string orderReference, string packageName)
        {
            try
            {
                await _emailService.SendOrderConfirmationEmail(email, companyName, orderReference,
                    packageName, amount);

                // Mark as paid for demo purposes
                order.PaymentStatus = "Paid";
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            // TODO: Implement PayPal integration
            TempData["SuccessMessage"] = "Payment processed successfully!";
            return RedirectToAction("PaymentSuccess");
        }

        private async Task<IActionResult> RedirectToYoco(Order order, string service,
            string package, decimal amount, string email, string companyName,
            string orderReference, string packageName)
        {
            try
            {
                await _emailService.SendOrderConfirmationEmail(email, companyName, orderReference,
                    packageName, amount);

                // Mark as paid for demo purposes
                order.PaymentStatus = "Paid";
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            // TODO: Implement Yoco integration
            TempData["SuccessMessage"] = "Payment processed successfully!";
            return RedirectToAction("PaymentSuccess");
        }

        [Authorize]
        public IActionResult BankTransferDetails()
        {
            return View();
        }

        [Authorize]
        public IActionResult PaymentSuccess()
        {
            return View();
        }

        private string GetPackageName(string service, string package)
        {
            var serviceName = service switch
            {
                "website" => "Website Development",
                "ecommerce" => "E-commerce",
                "branding" => "Brand Identity",
                "seo" => "SEO Optimization",
                _ => service
            };

            var packageTier = package switch
            {
                "basic" => "Basic",
                "standard" => "Standard",
                "premium" => "Premium",
                "starter" => "Starter",
                "professional" => "Professional",
                "local" => "Local",
                "advanced" => "Advanced",
                _ => package
            };

            return $"{serviceName} - {packageTier}";
        }
    }
}