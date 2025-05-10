namespace TravelApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required int Age { get; set; }
        public bool UserType {  get; set; }
        public required string RegistrationIp { get; set; }
        public required string LastIp { get; set; }
        public string? AvatarLink { get; set; }
    }
}
