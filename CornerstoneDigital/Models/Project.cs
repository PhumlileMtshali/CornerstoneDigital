using System.ComponentModel.DataAnnotations;

namespace CornerstoneDigital.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Client { get; set; }

        public string Duration { get; set; }

        [Required]
        public string Challenge { get; set; }

        [Required]
        public string Solution { get; set; }

        public string Results { get; set; } // JSON array

        public string Technologies { get; set; } // JSON array

        public string ImageIcon { get; set; }

        public string SlugUrl { get; set; } // e.g., "tech-startup"

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}