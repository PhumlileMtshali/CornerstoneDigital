using System.ComponentModel.DataAnnotations;

namespace CornerstoneDigital.Models
{
    public class ServicePackage
    {
        public int Id { get; set; }

        [Required]
        public string ServiceType { get; set; } // website, ecommerce, branding, seo

        [Required]
        public string PackageName { get; set; } // basic, standard, premium, etc.

        [Required]
        [Range(0, 999999)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Features { get; set; } // JSON array

        public string DeliveryTime { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}