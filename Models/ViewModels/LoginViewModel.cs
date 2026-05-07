using System.ComponentModel.DataAnnotations;

namespace BarangaySystem.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "That is not a valid email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}