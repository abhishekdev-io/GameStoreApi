using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(90, MinimumLength = 9, ErrorMessage = "Username must be 9-50 characters")]
        public required string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public required string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; } = string.Empty;

    }
}
