using System.ComponentModel.DataAnnotations;

namespace BarangaySystem.Models
{
    public class DocumentType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";
        public string Description { get; set;} = "";
        public decimal Fee { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public ICollection<DocumentRequest> Requests { get; set; } = new List<DocumentRequest>();

    }
}