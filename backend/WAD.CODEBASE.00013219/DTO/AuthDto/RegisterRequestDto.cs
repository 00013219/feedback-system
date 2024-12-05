using System.ComponentModel.DataAnnotations;

namespace WAD.CODEBASE._00013219.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name should be between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        
        public string Role { get; set; } = "user";
        
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}