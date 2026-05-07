using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BarangaySystem.Data;
using BarangaySystem.Models;

namespace BarangaySystem.Controllers
{
    [Authorize(Roles = "Admin")] //EVERY action here requires Admin Role
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard with summary counts
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.Total = await _context.DocumentRequests.CountAsync();
            ViewBag.Pending = await _context.DocumentRequests.CountAsync(r => r.Status == "Pending");
            ViewBag.Processing = await _context.DocumentRequests.CountAsync(r => r.Status == "Processing");
            ViewBag.Ready = await _context.DocumentRequests.CountAsync(r => r.Status == "Ready for Pickup");
            ViewBag.Today = await _context.DocumentRequests.CountAsync(r => r.RequestDate.Date == DateTime.Today);
            return View();
        }

        // All requests with search + filter
        public async Task<IActionResult> AllRequests(string? name = null, string? status = null, int? typeId = null)
        {
           // Start with all requests
           var query = _context.DocumentRequests
               .Include(r => r.User)
               .Include(r => r.DocumentType)
               .AsQueryable();

            // Apply filters only if the user typed something
            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.User.FullName.Contains(name));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);
            
            if (typeId.HasValue)
                query = query.Where(r => r.DocumentTypeId == typeId);
            
            ViewBag.DocumentTypes = await _context.DocumentTypes.ToListAsync();
            return View(await query.OrderByDescending(r => r.RequestDate).ToListAsync());
        }

        // View one request's full details
        public async Task<IActionResult> Detail(int id)
        {
            var request = await _context.DocumentRequests
                .Include(r => r.User)
                .Include(r => r.DocumentType)
                .Include(r => r.AuditLogs)
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (request == null) return NotFound();
            return View(request);
        }

        // Update the status of a request
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string newStatus, string remarks)
        {
            var request = await _context.DocumentRequests.FindAsync(id);
            if (request == null) return NotFound();

            string oldStatus = request.Status;
            request.Status = newStatus;
            request.LastUpdated = DateTime.Now;
            request.AdminRemarks = remarks;

            // Write an audit log entry
            _context.AuditLogs.Add(new AuditLog
            {
               DocumentRequestId = request.Id,
               ChangedByName = User.Identity?.Name ?? "Admin",
               OldStatus = oldStatus,
               NewStatus = newStatus,
               Remarks = remarks 
            });

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Status updated to: {newStatus}";
            return RedirectToAction("Detail", new { id });
        }

        // Reports Page
        public async Task<IActionResult> Reports(string period = "monthly")
        {
            var today = DateTime.Today;

            //Get all requests, then filter by the chosen period
            var all = _context.DocumentRequests.Include(r => r.DocumentType).AsQueryable();

            var filtered = period switch
            {
                "daily" => all.Where(r => r.RequestDate.Date == today),
                "weekly" => all.Where(r => r.RequestDate >= today.AddDays(-7)),
                _  => all.Where(r => r.RequestDate.Month == today.Month && r.RequestDate.Year == today.Year)
            };

            var list = await filtered.ToListAsync();

            ViewBag.Period = period;
            ViewBag.Total = list.Count;
            ViewBag.Pending = list.Count(r => r.Status == "Pending");
            ViewBag.Processing = list.Count(r => r.Status == "Processing");
            ViewBag.Ready = list.Count(r => r.Status == "Ready for Pickup");
            ViewBag.Released = list.Count(r => r.Status == "Released");
            ViewBag.Rejected = list.Count(r => r.Status == "Rejected");

            // Find the most requested document type
            ViewBag.TopType = list
                .GroupBy(r => r.DocumentType?.Name ?? "Unknown")
                .OrderByDescending(g => g.Count())
                .Select(g => $"{g.Key} ({g.Count()} requests)")
                .FirstOrDefault() ?? "None yet";
            
            return View(list);
        }
    }
}