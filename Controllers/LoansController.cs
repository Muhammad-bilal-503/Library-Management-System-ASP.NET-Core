using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Data;
using LibraryMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryMS.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly AppDbContext _db;

        public LoansController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? status)
        {
            var query = _db.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .AsQueryable();

            if (status == "active") query = query.Where(l => !l.IsReturned);
            else if (status == "returned") query = query.Where(l => l.IsReturned);
            else if (status == "overdue") query = query.Where(l => !l.IsReturned && DateTime.Now > l.DueDate);

            ViewBag.Status = status;
            return View(await query.OrderByDescending(l => l.IssueDate).ToListAsync());
        }

        public async Task<IActionResult> Issue()
        {
            ViewBag.Books = new SelectList(
                await _db.Books.Where(b => b.AvailableCopies > 0).OrderBy(b => b.Title).ToListAsync(),
                "Id", "Title");
            ViewBag.Members = new SelectList(
                await _db.Members.Where(m => m.IsActive).OrderBy(m => m.FullName).ToListAsync(),
                "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Issue(Loan loan)
        {
            var book = await _db.Books.FindAsync(loan.BookId);
            var member = await _db.Members
                .Include(m => m.Loans)
                .FirstOrDefaultAsync(m => m.Id == loan.MemberId);

            if (book == null || member == null)
            {
                TempData["Error"] = "Invalid book or member.";
                return RedirectToAction(nameof(Issue));
            }

            if (book.AvailableCopies <= 0)
            {
                TempData["Error"] = $"'{book.Title}' is not available.";
                return RedirectToAction(nameof(Issue));
            }

            if (member.ActiveLoansCount >= 3)
            {
                TempData["Error"] = "Member already has 3 active loans (maximum limit).";
                return RedirectToAction(nameof(Issue));
            }

            loan.IssueDate = DateTime.Now;
            loan.DueDate = DateTime.Now.AddDays(14);
            loan.IsReturned = false;

            book.AvailableCopies--;

            _db.Loans.Add(loan);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"'{book.Title}' issued to {member.FullName}. Due: {loan.DueDate:dd MMM yyyy}";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            var loan = await _db.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null) return NotFound();

            loan.IsReturned = true;
            loan.ReturnDate = DateTime.Now;
            loan.Book.AvailableCopies++;

            await _db.SaveChangesAsync();

            var fineMsg = loan.Fine > 0 ? $" Fine: Rs. {loan.Fine}" : "";
            TempData["Success"] = $"'{loan.Book.Title}' returned by {loan.Member.FullName}.{fineMsg}";
            return RedirectToAction(nameof(Index));
        }
    }
}
