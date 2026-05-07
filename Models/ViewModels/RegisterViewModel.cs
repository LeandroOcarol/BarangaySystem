using System.ComponentModel.DataAnnotations;

namespace BarangaySystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required] public string FullName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = "";

        [Required] public string Address { get; set; } = "";
        [Required] public string ContactNumber { get; set; } = "";
    }
}