using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = "";

        [Required, MaxLength(150)]
        public string Author { get; set; } = "";

        [Required, MaxLength(20)]
        public string ISBN { get; set; } = "";

        [MaxLength(100)]
        public string Genre { get; set; } = "";

        [MaxLength(100)]
        public string Publisher { get; set; } = "";

        public int PublishedYear { get; set; }

        public int TotalCopies { get; set; } = 1;

        public int AvailableCopies { get; set; } = 1;

        [MaxLength(500)]
        public string Description { get; set; } = "";

        // Book cover image URL from Open Library API
        public string? CoverImageUrl { get; set; }

        public string? CoverImageId { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        // OOP: Computed property (Encapsulation)
        public bool IsAvailable => AvailableCopies > 0;

        public string StatusBadge => IsAvailable ? "Available" : "All Checked Out";
    }
}
