using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Models;

namespace LibraryMS.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Loan> Loans => Set<Loan>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Loan>()
                .HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId);

            builder.Entity<Loan>()
                .HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId);

            // Seed books with Open Library cover IDs
            builder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Clean Code", Author = "Robert C. Martin", ISBN = "978-0132350884", Genre = "Technology", Publisher = "Prentice Hall", PublishedYear = 2008, TotalCopies = 3, AvailableCopies = 3, Description = "A handbook of agile software craftsmanship. Every developer should read this masterpiece.", CoverImageId = "OL7353123M", AddedDate = DateTime.Now },
                new Book { Id = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt", ISBN = "978-0201616224", Genre = "Technology", Publisher = "Addison-Wesley", PublishedYear = 1999, TotalCopies = 2, AvailableCopies = 2, Description = "Your journey to mastery. A must-read for every serious programmer.", CoverImageId = "OL7676592M", AddedDate = DateTime.Now },
                new Book { Id = 3, Title = "Design Patterns", Author = "Gang of Four", ISBN = "978-0201633610", Genre = "Technology", Publisher = "Addison-Wesley", PublishedYear = 1994, TotalCopies = 2, AvailableCopies = 1, Description = "Elements of Reusable Object-Oriented Software. The classic patterns bible.", CoverImageId = "OL1429049M", AddedDate = DateTime.Now },
                new Book { Id = 4, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "978-0743273565", Genre = "Fiction", Publisher = "Scribner", PublishedYear = 1925, TotalCopies = 5, AvailableCopies = 4, Description = "A story of the fabulously wealthy Jay Gatsby and his love for the beautiful Daisy Buchanan.", CoverImageId = "OL7353123M", AddedDate = DateTime.Now },
                new Book { Id = 5, Title = "Sapiens", Author = "Yuval Noah Harari", ISBN = "978-0062316097", Genre = "History", Publisher = "Harper", PublishedYear = 2011, TotalCopies = 4, AvailableCopies = 3, Description = "A brief history of humankind. From stone age to the modern era.", CoverImageId = "OL25972162M", AddedDate = DateTime.Now },
                new Book { Id = 6, Title = "Atomic Habits", Author = "James Clear", ISBN = "978-0735211292", Genre = "Self-Help", Publisher = "Avery", PublishedYear = 2018, TotalCopies = 3, AvailableCopies = 2, Description = "Tiny changes, remarkable results. Build good habits and break bad ones.", CoverImageId = "OL27258170M", AddedDate = DateTime.Now }
            );

            builder.Entity<Member>().HasData(
                new Member { Id = 1, FullName = "Ahmed Khan", Email = "ahmed@example.com", Phone = "0300-1234567", MembershipId = "MEM-001", Address = "Block 5, Islamabad", MembershipType = MembershipType.Student, MembershipExpiry = DateTime.Now.AddYears(1), IsActive = true, CreatedAt = DateTime.Now },
                new Member { Id = 2, FullName = "Fatima Malik", Email = "fatima@example.com", Phone = "0321-9876543", MembershipId = "MEM-002", Address = "F-7, Islamabad", MembershipType = MembershipType.Premium, MembershipExpiry = DateTime.Now.AddYears(2), IsActive = true, CreatedAt = DateTime.Now }
            );
        }
    }
}
