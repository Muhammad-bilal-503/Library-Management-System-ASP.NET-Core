using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Models
{
    // Base class demonstrating INHERITANCE
    public abstract class Person
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string Phone { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Abstract method for POLYMORPHISM
        public abstract string GetRole();

        public string GetDisplayInfo() => $"{FullName} ({Email})";
    }

    public class Member : Person
    {
        [MaxLength(20)]
        public string MembershipId { get; set; } = "";

        [MaxLength(300)]
        public string Address { get; set; } = "";

        public MembershipType MembershipType { get; set; } = MembershipType.Regular;

        public DateTime MembershipExpiry { get; set; } = DateTime.Now.AddYears(1);

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        // POLYMORPHISM - overriding abstract method
        public override string GetRole() => $"Member ({MembershipType})";

        // Encapsulation
        public bool IsMembershipValid => IsActive && MembershipExpiry >= DateTime.Now;

        public int ActiveLoansCount => Loans?.Count(l => !l.IsReturned) ?? 0;
    }

    public enum MembershipType
    {
        Regular,
        Student,
        Premium,
        Staff
    }
}
