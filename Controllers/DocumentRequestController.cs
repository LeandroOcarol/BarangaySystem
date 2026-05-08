using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BarangaySystem.Data;
using BarangaySystem.Models;
using BarangaySystem.Services;

namespace BarangaySystem.Controllers
{
    [Authorize] //All actions need login
    public class DocumentRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PdfService _pdfService;

        public DocumentRequestController(ApplicationDbContext context, PdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        // Get the ID of the currently logged-in user
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Resident: See my own requests
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> MyRequests()
        {
            var userId = GetUserId();

            var unseenReadyRequests = await _context.DocumentRequests
                .Where(r => r.UserId == userId && r.Status == "Ready for Pickup" && !r.IsReadyNotificationSeen)
                .ToListAsync();

            if (unseenReadyRequests.Any())
            {
                foreach (var request in unseenReadyRequests)
                {
                    request.IsReadyNotificationSeen = true;
                }

                await _context.SaveChangesAsync();
            }

            var myRequests = await _context.DocumentRequests
                .Include(r => r.DocumentType)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RequestDate)
                .ToListAsync();
            
            return View(myRequests);
        }

        // Resident: Show submit form
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> Submit()
        {
            // Pass document types to the dropdown in the form
            ViewBag.DocumentTypes = await _context.DocumentTypes
                .Where(dt => dt.IsActive)
                .ToListAsync();
            return View();
        }

        //Resident: Save the submitted request
        [HttpPost]
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> Submit(int documentTypeId, string purpose)
        {
           var request = new DocumentRequest
           {
               UserId = GetUserId(),
               DocumentTypeId = documentTypeId,
               Purpose = purpose,
               Status = "Pending",
               ReferenceNumber = GenerateRefNumber()
           };

           _context.DocumentRequests.Add(request);
           await _context.SaveChangesAsync();

           TempData["Success"] = $"Request submitted! Your reference:{request.ReferenceNumber}";
           return RedirectToAction("MyRequests");
        }

        // Resident: Download PDF (only if approved)
        [Authorize(Roles = "Resident")]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var request = await _context.DocumentRequests
                .Include(r => r.User)
                .Include(r => r.DocumentType)
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == GetUserId());
            
            if (request == null) return NotFound();

            // Only allow download if status is Ready or Released
            if (request.Status != "Ready for Pickup" && request.Status != "Released")
            {
                TempData["Error"] = "PDF is only available when your request is approved.";
                return RedirectToAction("MyRequests");
            }

            byte[] pdf = _pdfService.GeneratePdf(request);
            return File(pdf, "application/pdf", $"{request.ReferenceNumber}.pdf");
        }

        // Helper: Make unique reference number
        private string GenerateRefNumber()
        {
            int count = _context.DocumentRequests.Count() + 1;
            return $"BRG-{DateTime.Now.Year}-{count:D4}"; // BRG-2024-0001
        }
    }
}