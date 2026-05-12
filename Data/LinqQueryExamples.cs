namespace BarangaySystem.Data
{
    public static class LinqQueryExamples
    {
        // These LINQ strings correspond to the SQL strings in SqlQueryExamples.cs.
        // Use them for teaching side-by-side with raw SQL.

        public const string GetUserByEmail =
            "var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);";

        public const string GetRequestsByUser =
            "var myRequests = await _context.DocumentRequests"
            + "\n    .Include(r => r.DocumentType)"
            + "\n    .Where(r => r.UserId == userId)"
            + "\n    .OrderByDescending(r => r.RequestDate)"
            + "\n    .ToListAsync();";

        public const string GetActiveDocumentTypes =
            "var documentTypes = await _context.DocumentTypes"
            + "\n    .Where(dt => dt.IsActive)"
            + "\n    .ToListAsync();";

        public const string GetRequestDetail =
            "var request = await _context.DocumentRequests"
            + "\n    .Include(r => r.User)"
            + "\n    .Include(r => r.DocumentType)"
            + "\n    .Include(r => r.AuditLogs)"
            + "\n    .FirstOrDefaultAsync(r => r.Id == id);";

        public const string SearchRequestsBase =
            "var query = _context.DocumentRequests"
            + "\n    .Include(r => r.User)"
            + "\n    .Include(r => r.DocumentType)"
            + "\n    .AsQueryable();";

        public const string SearchRequestsByName =
            "query = query.Where(r => r.User.FullName.Contains(name));";

        public const string SearchRequestsByStatus =
            "query = query.Where(r => r.Status == status);";

        public const string SearchRequestsByDocumentType =
            "query = query.Where(r => r.DocumentTypeId == typeId);";

        public const string CountAllRequests =
            "var total = await _context.DocumentRequests.CountAsync();";

        public const string CountRequestsByStatus =
            "var pending = await _context.DocumentRequests.CountAsync(r => r.Status == \"Pending\");";

        public const string CountTodaysRequests =
            "var todayCount = await _context.DocumentRequests.CountAsync(r => r.RequestDate.Date == DateTime.Today);";

        public const string InsertDocumentRequest =
            "var request = new DocumentRequest { ... };"
            + "\n_context.DocumentRequests.Add(request);"
            + "\nawait _context.SaveChangesAsync();";

        public const string UpdateRequestStatus =
            "var request = await _context.DocumentRequests.FindAsync(id);"
            + "\nrequest.Status = newStatus;"
            + "\nrequest.LastUpdated = DateTime.Now;"
            + "\nrequest.AdminRemarks = remarks;"
            + "\nawait _context.SaveChangesAsync();";

        public const string InsertAuditLog =
            "_context.AuditLogs.Add(new AuditLog { ... });"
            + "\nawait _context.SaveChangesAsync();";

        public const string GetReportPeriod =
            "var all = _context.DocumentRequests.Include(r => r.DocumentType).AsQueryable();"
            + "\nvar filtered = period switch"
            + "\n{"
            + "\n    \"daily\" => all.Where(r => r.RequestDate.Date == today),"
            + "\n    \"weekly\" => all.Where(r => r.RequestDate >= today.AddDays(-7)),"
            + "\n    _ => all.Where(r => r.RequestDate.Month == today.Month && r.RequestDate.Year == today.Year)"
            + "\n};";

        public const string GetTopDocumentType =
            "var topType = list"
            + "\n    .GroupBy(r => r.DocumentType?.Name ?? \"Unknown\")"
            + "\n    .OrderByDescending(g => g.Count())"
            + "\n    .Select(g => $\"{g.Key} ({g.Count()} requests)\")"
            + "\n    .FirstOrDefault();";

        public const string AddDocumentType =
            "_context.DocumentTypes.Add(model);"
            + "\nawait _context.SaveChangesAsync();";

        public const string UpdateDocumentType =
            "var type = await _context.DocumentTypes.FindAsync(model.Id);"
            + "\ntype.Name = model.Name;"
            + "\ntype.Description = model.Description;"
            + "\ntype.Fee = model.Fee;"
            + "\ntype.IsActive = model.IsActive;"
            + "\nawait _context.SaveChangesAsync();";

        public const string DeleteDocumentType =
            "var type = await _context.DocumentTypes"
            + "\n    .Include(t => t.Requests)"
            + "\n    .FirstOrDefaultAsync(t => t.Id == id);"
            + "\n_context.DocumentTypes.Remove(type);"
            + "\nawait _context.SaveChangesAsync();";

        public const string CountRequestsForDocumentType =
            "var requestCount = await _context.DocumentRequests.CountAsync(r => r.DocumentTypeId == id);";

        public const string RegisterUser =
            "var user = new User { FullName = model.FullName, Email = model.Email, PasswordHash = hashed, Address = model.Address, ContactNumber = model.ContactNumber, Role = \"Resident\" };"
            + "\n_context.Users.Add(user);"
            + "\nawait _context.SaveChangesAsync();";
    }
}
