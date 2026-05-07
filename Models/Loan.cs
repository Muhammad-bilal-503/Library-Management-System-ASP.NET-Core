using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Models
{
    public class Loan
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public DateTime IssueDate { get; set; } = DateTime.Now;

        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;

        [MaxLength(500)]
        public string Notes { get; set; } = "";

        // Encapsulation - computed properties
        public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;

        public int DaysOverdue => IsOverdue ? (int)(DateTime.Now - DueDate).TotalDays : 0;

        public decimal Fine => IsOverdue ? DaysOverdue * 10m : 0; // Rs. 10 per day

        public string Status
        {
            get
            {
                if (IsReturned) return "Returned";
                if (IsOverdue) return "Overdue";
                return "Active";
            }
        }
    }
}
