using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Data;

namespace LibraryMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalBooks = await _db.Books.CountAsync();
            ViewBag.TotalMembers = await _db.Members.CountAsync();
            ViewBag.ActiveLoans = await _db.Loans.CountAsync(l => !l.IsReturned);
            ViewBag.OverdueLoans = await _db.Loans.CountAsync(l => !l.IsReturned && DateTime.Now > l.DueDate);
            ViewBag.AvailableBooks = await _db.Books.CountAsync(b => b.AvailableCopies > 0);

            var recentBooks = await _db.Books
                .OrderByDescending(b => b.AddedDate)
                .Take(6)
                .ToListAsync();

            var recentLoans = await _db.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => !l.IsReturned)
                .OrderByDescending(l => l.IssueDate)
                .Take(5)
                .ToListAsync();

            ViewBag.RecentLoans = recentLoans;

            return View(recentBooks);
        }
    }
}
