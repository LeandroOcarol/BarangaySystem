namespace BarangaySystem.Data
{
    public static class SqlQueryExamples
    {
        // These SQL strings are teaching examples that correspond to the LINQ queries used in the app.
        // They are not executed by the app currently, but they can be shown to students for learning.

        public const string GetUserByEmail =
            "SELECT TOP(1) * FROM Users WHERE Email = @email";

        public const string GetRequestsByUser =
            "SELECT r.* FROM DocumentRequests r " +
            "INNER JOIN DocumentTypes dt ON dt.Id = r.DocumentTypeId " +
            "WHERE r.UserId = @userId " +
            "ORDER BY r.RequestDate DESC";

        public const string GetActiveDocumentTypes =
            "SELECT * FROM DocumentTypes WHERE IsActive = 1";

        public const string GetRequestDetail =
            "SELECT r.* FROM DocumentRequests r " +
            "INNER JOIN Users u ON u.Id = r.UserId " +
            "INNER JOIN DocumentTypes dt ON dt.Id = r.DocumentTypeId " +
            "WHERE r.Id = @id";

        public const string SearchRequestsBase =
            "SELECT r.* FROM DocumentRequests r " +
            "INNER JOIN Users u ON u.Id = r.UserId " +
            "INNER JOIN DocumentTypes dt ON dt.Id = r.DocumentTypeId";

        public const string SearchRequestsByName =
            "SELECT r.* FROM DocumentRequests r " +
            "INNER JOIN Users u ON u.Id = r.UserId " +
            "INNER JOIN DocumentTypes dt ON dt.Id = r.DocumentTypeId " +
            "WHERE u.FullName LIKE '%' + @name + '%'";

        public const string SearchRequestsByStatus =
            "SELECT r.* FROM DocumentRequests r " +
            "INNER JOIN Users u ON u.Id = r.UserId " +
            "INNER JOIN DocumentTypes dt ON dt.Id = r.DocumentTypeId " +
            "WHERE r.Status = @status";

        public const string SearchRequestsByDocumentType =
            "SELECT r.* FROM DocumentRequests r " +
            "INNER JOIN Users u ON u.Id = r.UserId " +
            "INNER JOIN DocumentTypes dt ON dt.Id = r.DocumentTypeId " +
            "WHERE r.DocumentTypeId = @typeId";

        public const string CountAllRequests =
            "SELECT COUNT(*) FROM DocumentRequests";

        public const string CountRequestsByStatus =
            "SELECT COUNT(*) FROM DocumentRequests WHERE Status = @status";

        public const string CountTodaysRequests =
            "SELECT COUNT(*) FROM DocumentRequests WHERE CAST(RequestDate AS DATE) = CAST(GETDATE() AS DATE)";

        public const string InsertDocumentRequest =
            "INSERT INTO DocumentRequests (ReferenceNumber, UserId, DocumentTypeId, Status, Purpose, RequestDate, IsReadyNotificationSeen) " +
            "VALUES (@referenceNumber, @userId, @documentTypeId, @status, @purpose, @requestDate, @isReadyNotificationSeen)";

        public const string UpdateRequestStatus =
            "UPDATE DocumentRequests SET Status = @newStatus, LastUpdated = @lastUpdated, AdminRemarks = @remarks, IsReadyNotificationSeen = @isReadySeen WHERE Id = @id";

        public const string InsertAuditLog =
            "INSERT INTO AuditLogs (DocumentRequestId, ChangedByName, OldStatus, NewStatus, Remarks, ChangedAt) " +
            "VALUES (@requestId, @changedByName, @oldStatus, @newStatus, @remarks, @changedAt)";

        public const string GetReportPeriod =
            "SELECT r.* FROM DocumentRequests r " +
            "INNER JOIN DocumentTypes dt ON dt.Id = r.DocumentTypeId " +
            "WHERE r.RequestDate >= @startDate AND r.RequestDate < @endDate";

        public const string GetTopDocumentType =
            "SELECT dt.Name, COUNT(*) AS RequestCount FROM DocumentRequests r " +
            "INNER JOIN DocumentTypes dt ON dt.Id = r.DocumentTypeId " +
            "WHERE r.RequestDate >= @startDate AND r.RequestDate < @endDate " +
            "GROUP BY dt.Name " +
            "ORDER BY COUNT(*) DESC";

        public const string AddDocumentType =
            "INSERT INTO DocumentTypes (Name, Description, Fee, IsActive) VALUES (@name, @description, @fee, @isActive)";

        public const string UpdateDocumentType =
            "UPDATE DocumentTypes SET Name = @name, Description = @description, Fee = @fee, IsActive = @isActive WHERE Id = @id";

        public const string DeleteDocumentType =
            "DELETE FROM DocumentTypes WHERE Id = @id";

        public const string CountRequestsForDocumentType =
            "SELECT COUNT(*) FROM DocumentRequests WHERE DocumentTypeId = @id";

        public const string RegisterUser =
            "INSERT INTO Users (FullName, Email, PasswordHash, Address, ContactNumber, Role, CreatedAt) " +
            "VALUES (@fullName, @email, @passwordHash, @address, @contactNumber, @role, @createdAt)";
    }
}
