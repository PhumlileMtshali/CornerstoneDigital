using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CornerstoneDigital.Controllers
{
    public class SetupController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SetupController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> AssignAdminRole()
        {
            var result = "";

            // 1. Check if Admin role exists
            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            result += $"Admin role exists: {roleExists}\n";

            if (!roleExists)
            {
                var createRole = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                result += $"Created Admin role: {createRole.Succeeded}\n";
            }

            // 2. Find the admin user
            var adminUser = await _userManager.FindByEmailAsync("admin@cornerstonedigital.com");
            if (adminUser == null)
            {
                result += "❌ Admin user not found!\n";
                return Content(result);
            }

            result += $"✅ Found user: {adminUser.Email}\n";

            // 3. Check current roles
            var currentRoles = await _userManager.GetRolesAsync(adminUser);
            result += $"Current roles: {string.Join(", ", currentRoles)}\n";

            // 4. Add to Admin role
            if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                var addToRole = await _userManager.AddToRoleAsync(adminUser, "Admin");
                result += $"Added to Admin role: {addToRole.Succeeded}\n";

                if (!addToRole.Succeeded)
                {
                    result += $"Errors: {string.Join(", ", addToRole.Errors.Select(e => e.Description))}\n";
                }
            }
            else
            {
                result += "User already in Admin role\n";
            }

            // 5. Verify
            var finalRoles = await _userManager.GetRolesAsync(adminUser);
            result += $"✅ Final roles: {string.Join(", ", finalRoles)}\n";

            return Content(result);
        }
    }
}