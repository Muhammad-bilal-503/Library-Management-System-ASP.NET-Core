using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Data;
using LibraryMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryMS.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<BooksController> _logger;

        // Constructor Injection (Encapsulation + OOP)
        public BooksController(AppDbContext db, ILogger<BooksController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // READ - List all books with search/filter
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? search, string? genre, string? availability)
        {
            var query = _db.Books.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search) || b.ISBN.Contains(search));

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(b => b.Genre == genre);

            if (availability == "available")
                query = query.Where(b => b.AvailableCopies > 0);
            else if (availability == "unavailable")
                query = query.Where(b => b.AvailableCopies == 0);

            var books = await query.OrderBy(b => b.Title).ToListAsync();
            var genres = await _db.Books.Select(b => b.Genre).Distinct().OrderBy(g => g).ToListAsync();

            ViewBag.Genres = genres;
            ViewBag.Search = search;
            ViewBag.Genre = genre;
            ViewBag.Availability = availability;

            return View(books);
        }

        // READ - Book details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _db.Books
                .Include(b => b.Loans)
                .ThenInclude(l => l.Member)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();
            return View(book);
        }

        // CREATE - Show form
        public IActionResult Create() => View();

        // CREATE - Handle form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid) return View(book);

            try
            {
                book.AvailableCopies = book.TotalCopies;
                book.AddedDate = DateTime.Now;

                // Fetch cover from Open Library API
                if (!string.IsNullOrEmpty(book.ISBN))
                {
                    book.CoverImageUrl = $"https://covers.openlibrary.org/b/isbn/{book.ISBN.Replace("-", "")}-M.jpg";
                }

                _db.Books.Add(book);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Book created: {Title}", book.Title);
                TempData["Success"] = $"Book '{book.Title}' added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating book");
                ModelState.AddModelError("", "Error saving book. Please try again.");
                return View(book);
            }
        }

        // UPDATE - Show form
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // UPDATE - Handle form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id) return BadRequest();
            if (!ModelState.IsValid) return View(book);

            try
            {
                var existing = await _db.Books.FindAsync(id);
                if (existing == null) return NotFound();

                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.ISBN = book.ISBN;
                existing.Genre = book.Genre;
                existing.Publisher = book.Publisher;
                existing.PublishedYear = book.PublishedYear;
                existing.TotalCopies = book.TotalCopies;
                existing.Description = book.Description;
                existing.CoverImageUrl = $"https://covers.openlibrary.org/b/isbn/{book.ISBN.Replace("-", "")}-M.jpg";

                await _db.SaveChangesAsync();
                TempData["Success"] = "Book updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating book {Id}", id);
                ModelState.AddModelError("", "Error updating. Please try again.");
                return View(book);
            }
        }

        // DELETE - Confirm page
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // DELETE - Handle deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return NotFound();

            if (book.AvailableCopies < book.TotalCopies)
            {
                TempData["Error"] = "Cannot delete book with active loans!";
                return RedirectToAction(nameof(Index));
            }

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Book '{book.Title}' deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
