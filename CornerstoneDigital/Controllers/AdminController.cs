using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CornerstoneDigital.Data;
using CornerstoneDigital.Models;
using Microsoft.AspNetCore.Identity;

namespace CornerstoneDigital.Controllers
{
    [Authorize(Roles = "Admin")] // Only admins can access
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ========== DIAGNOSTIC - TEMPORARY ==========
        [AllowAnonymous] // Temporarily allow anyone to check
        public async Task<IActionResult> CheckRole()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content("❌ NOT LOGGED IN");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Content("❌ USER NOT FOUND");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var isInAdminRole = await _userManager.IsInRoleAsync(user, "Admin");

            var result = $@"
✅ USER AUTHENTICATED
Email: {user.Email}
UserName: {user.UserName}
User Id: {user.Id}

ROLES ASSIGNED: {(roles.Any() ? string.Join(", ", roles) : "NONE")}
Is In Admin Role: {isInAdminRole}

Claims:
{string.Join("\n", User.Claims.Select(c => $"  - {c.Type}: {c.Value}"))}
            ";

            return Content(result);
        }

        // Dashboard Home
        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalProjects = await _context.Projects.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.PaymentStatus == "Pending"),
                TotalRevenue = await _context.Orders.Where(o => o.PaymentStatus == "Paid").SumAsync(o => o.Amount),
                RecentOrders = await _context.Orders.OrderByDescending(o => o.OrderDate).Take(5).ToListAsync()
            };

            return View(stats);
        }

        // ========== PROJECTS MANAGEMENT ==========

        public async Task<IActionResult> Projects()
        {
            var projects = await _context.Projects.OrderByDescending(p => p.CreatedDate).ToListAsync();
            return View(projects);
        }

        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Project created successfully!";
                return RedirectToAction(nameof(Projects));
            }
            return View(project);
        }

        public async Task<IActionResult> EditProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> EditProject(Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Project updated successfully!";
                return RedirectToAction(nameof(Projects));
            }
            return View(project);
        }

        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Project deleted successfully!";
            }
            return RedirectToAction(nameof(Projects));
        }

        // ========== PACKAGES MANAGEMENT ==========

        public async Task<IActionResult> Packages()
        {
            var packages = await _context.ServicePackages.OrderBy(p => p.ServiceType).ThenBy(p => p.Price).ToListAsync();
            return View(packages);
        }

        public IActionResult CreatePackage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePackage(ServicePackage package)
        {
            if (ModelState.IsValid)
            {
                _context.ServicePackages.Add(package);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Package created successfully!";
                return RedirectToAction(nameof(Packages));
            }
            return View(package);
        }

        public async Task<IActionResult> EditPackage(int id)
        {
            var package = await _context.ServicePackages.FindAsync(id);
            if (package == null) return NotFound();
            return View(package);
        }

        [HttpPost]
        public async Task<IActionResult> EditPackage(ServicePackage package)
        {
            if (ModelState.IsValid)
            {
                _context.ServicePackages.Update(package);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Package updated successfully!";
                return RedirectToAction(nameof(Packages));
            }
            return View(package);
        }

        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = await _context.ServicePackages.FindAsync(id);
            if (package != null)
            {
                _context.ServicePackages.Remove(package);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Package deleted successfully!";
            }
            return RedirectToAction(nameof(Packages));
        }

        // ========== ORDERS MANAGEMENT ==========

        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders.OrderByDescending(o => o.OrderDate).ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.PaymentStatus = status;
                if (status == "Paid")
                {
                    order.CompletedDate = DateTime.Now;
                }
                await _context.SaveChangesAsync();
                TempData["Success"] = "Order status updated!";
            }
            return RedirectToAction(nameof(Orders));
        }

        // ========== TESTIMONIALS MANAGEMENT ==========

        public async Task<IActionResult> Testimonials()
        {
            var testimonials = await _context.Testimonials.OrderByDescending(t => t.CreatedDate).ToListAsync();
            return View(testimonials);
        }

        public IActionResult CreateTestimonial()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestimonial(Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Testimonials.Add(testimonial);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Testimonial created successfully!";
                return RedirectToAction(nameof(Testimonials));
            }
            return View(testimonial);
        }

        public async Task<IActionResult> EditTestimonial(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null) return NotFound();
            return View(testimonial);
        }

        [HttpPost]
        public async Task<IActionResult> EditTestimonial(Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Testimonials.Update(testimonial);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Testimonial updated successfully!";
                return RedirectToAction(nameof(Testimonials));
            }
            return View(testimonial);
        }

        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Testimonial deleted successfully!";
            }
            return RedirectToAction(nameof(Testimonials));
        }
    }
}