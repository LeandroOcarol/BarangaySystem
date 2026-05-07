using System.ComponentModel.DataAnnotations;

namespace BarangaySystem.Models
{
    public class User
    {
        public int Id { get; set; } //Auto-generated ID (Primary Key)

        [Required]
        public string FullName { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = ""; // Store hashed password(Never store plain password)

        public string Role { get; set; } = "Resident"; // Admin or Residents

        public string Address { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //This links User to their requests (one user = many requests)
        public ICollection<DocumentRequest> Requests { get; set; } = new List<DocumentRequest>();
    }
}