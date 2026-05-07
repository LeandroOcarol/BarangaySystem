namespace BarangaySystem.Models
{
    public class DocumentRequest
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; } = ""; // e.g. BRG-2024-0001

        //Foreign Key - connects this request to a User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        //Foreign Key - connects this request to a DocumentType
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; } = null!;

        public string Status { get; set; } = "Pending";
        //Allowed Values: Pending, Processing, Ready for Pickup, Released, Rejected

        public string Purpose { get; set; } = "";
        public string AdminRemarks { get; set; } = "";
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdated { get; set; }

        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}