using System.ComponentModel.DataAnnotations;

namespace CornerstoneDigital.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string OrderReference { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string ContactName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string ServiceType { get; set; }

        [Required]
        public string PackageName { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed

        public string ProjectDetails { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public DateTime? CompletedDate { get; set; }
    }
}
