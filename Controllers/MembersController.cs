using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Data;
using LibraryMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryMS.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly AppDbContext _db;

        public MembersController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? search)
        {
            var query = _db.Members.Include(m => m.Loans).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(m => m.FullName.Contains(search) || m.Email.Contains(search) || m.MembershipId.Contains(search));

            ViewBag.Search = search;
            return View(await query.OrderBy(m => m.FullName).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var member = await _db.Members
                .Include(m => m.Loans).ThenInclude(l => l.Book)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null) return NotFound();
            return View(member);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (!ModelState.IsValid) return View(member);

            var count = await _db.Members.CountAsync();
            member.MembershipId = $"MEM-{(count + 1):D3}";
            member.CreatedAt = DateTime.Now;

            _db.Members.Add(member);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Member '{member.FullName}' added! ID: {member.MembershipId}";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var member = await _db.Members.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member)
        {
            if (id != member.Id) return BadRequest();
            if (!ModelState.IsValid) return View(member);

            var existing = await _db.Members.FindAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = member.FullName;
            existing.Email = member.Email;
            existing.Phone = member.Phone;
            existing.Address = member.Address;
            existing.MembershipType = member.MembershipType;
            existing.MembershipExpiry = member.MembershipExpiry;
            existing.IsActive = member.IsActive;

            await _db.SaveChangesAsync();
            TempData["Success"] = "Member updated!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var member = await _db.Members.FindAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _db.Members
                .Include(m => m.Loans)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null) return NotFound();

            if (member.Loans.Any(l => !l.IsReturned))
            {
                TempData["Error"] = "Cannot delete member with active loans!";
                return RedirectToAction(nameof(Index));
            }

            _db.Members.Remove(member);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Member deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
