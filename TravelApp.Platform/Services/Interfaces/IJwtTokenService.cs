namespace TravelApp.Platform.Services.Interfaces
{
    public interface IJwtTokenService
    {
        public string GenerateToken(string email, bool role, int id, string flname);
        public string? GetUserIdFromToken(string token);
        public string? GetUserEmailFromToken(string token);
        public string? GetNameUserFromToken(string token);
    }
}
