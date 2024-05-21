using System.ComponentModel.DataAnnotations;

namespace ResumeApp.Models
{
    public class Candidates
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid mobile number")]

        public string? Mobile { get; set; }

        // Foreign key
        public int? DegreeId { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;

        // Navigation property
        public Degrees? Degrees { get; set; }
    }
}
