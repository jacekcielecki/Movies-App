using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models.Dtos
{
    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public int PermissionId { get; set; } = 1;
    }
}
