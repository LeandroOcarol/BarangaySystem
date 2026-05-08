using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BarangaySystem.Data;

namespace BarangaySystem.ViewComponents
{
    public class ReadyRequestsBadgeViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ReadyRequestsBadgeViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var principal = HttpContext.User as ClaimsPrincipal;
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                return Content(string.Empty);
            }

            if (!principal.IsInRole("Resident"))
            {
                return Content(string.Empty);
            }

            var userIdValue = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdValue, out var userId))
            {
                return Content(string.Empty);
            }

            var readyCount = await _context.DocumentRequests
                .CountAsync(r => r.UserId == userId && r.Status == "Ready for Pickup" && !r.IsReadyNotificationSeen);

            return View(readyCount);
        }
    }
}
