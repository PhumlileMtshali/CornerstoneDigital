using CornerstoneDigital.Data;
using CornerstoneDigital.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Database
// =======================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// =======================
// Identity (WITH ROLES)
// =======================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>() // ✅ Role support
.AddEntityFrameworkStores<ApplicationDbContext>();

// =======================
// Configure Identity Cookie
// =======================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

// =======================
// MVC
// =======================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// =======================
// Email Service
// =======================
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// =======================
// Middleware
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// =======================
// Routing
// =======================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// =======================
// Seed Admin User / Roles
// =======================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// =======================
// Post-Login Redirect Handler
// =======================
app.Use(async (context, next) =>
{
    await next();

    // Intercept successful login redirects
    if (context.Response.StatusCode == 302)
    {
        var location = context.Response.Headers["Location"].ToString();

        // If redirecting after login and user is authenticated
        if (context.User.Identity?.IsAuthenticated == true &&
            (location == "/" || location.Contains("ReturnUrl")))
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Response.Headers["Location"] = "/Admin";
            }
        }
    }
});

app.Run();

