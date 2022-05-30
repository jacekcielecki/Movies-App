namespace MoviesApp.Models.Dtos
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
    }
}
