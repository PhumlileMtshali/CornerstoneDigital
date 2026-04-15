using System.ComponentModel.DataAnnotations;

namespace CornerstoneDigital.Models
{
    public class Testimonial
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientName { get; set; }

        [Required]
        [StringLength(200)]
        public string Company { get; set; }

        [Required]
        [StringLength(1000)]
        public string Review { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}