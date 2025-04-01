using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class ClientIpService : IClientIpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ClientIpService> _logger;

        public ClientIpService(IHttpContextAccessor httpContextAccessor,
            ILogger<ClientIpService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public string GetClientIp(HttpContext context)
        {
            var ip = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
            ip ??= context.Request.Headers["X-Forwarded-For"].ToString()
                .Split(',')
                .Select(x => x.Trim())
                .FirstOrDefault();
            ip = ip == "" || ip == null ? context.Connection.RemoteIpAddress?.ToString() : "";

            return NormalizeIp(ip) ?? "Unknown";
        }

        public string GetClientIp()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                _logger.LogError("HttpContext is not available");
                return "Unknown";
            }
            return GetClientIp(context);
        }

        private static string NormalizeIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return null;
            if (ip.StartsWith("::ffff:")) ip = ip.Substring(7);
            else if (ip == "::1") ip = "127.0.0.1";
            else if (ip.Contains(":")) ip = ip.Split(':')[0];
            return ip;
        }
    }
}
