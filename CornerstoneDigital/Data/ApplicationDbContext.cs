using CornerstoneDigital.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CornerstoneDigital.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSets for our models
        public DbSet<Project> Projects { get; set; }
        public DbSet<ServicePackage> ServicePackages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
    }
}