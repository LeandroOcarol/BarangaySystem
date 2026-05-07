namespace BarangaySystem.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public int DocumentRequestId { get; set; }
        public DocumentRequest DocumentRequest { get; set; } = null!;

        public string ChangedByName { get; set; } = ""; //Who changed it
        public string OldStatus { get; set; } = ""; //What it was before
        public string NewStatus { get; set; } = ""; //What it was changed to
        public string Remarks { get; set; } = "";
        public DateTime ChangedAt { get; set; } = DateTime.Now;
    }
}