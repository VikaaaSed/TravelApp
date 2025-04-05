namespace TravelApp.Platform.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string email, bool role);
    }
}
