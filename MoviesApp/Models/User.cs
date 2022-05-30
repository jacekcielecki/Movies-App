namespace MoviesApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime Joined { get; set; }


        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
