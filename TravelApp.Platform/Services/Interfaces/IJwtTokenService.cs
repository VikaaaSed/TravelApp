namespace TravelApp.Platform.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string email, bool role, int id, string flname);
        string? GetUserIdFromToken(string token);
        string? GetNameUserFromToken(string token);
    }

}
