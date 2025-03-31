namespace TravelApp.Platform.ClientAPI
{
    public class LocationGalleryHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<LocationGalleryHttpClient> _logger;
        private readonly string BaseUrl = "https://localhost:7040/api";
        public LocationGalleryHttpClient(HttpClient httpClient,
            ILogger<LocationGalleryHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<API.Models.LocationGallery>> GetLocationGalleryByIdLocationAsync(int locationId)
        {
            try
            {
                if (locationId <= 0)
                {
                    _logger.LogWarning("Некорректный idLocation: {locationId}", locationId);
                    return [];
                }

                string url = $"{BaseUrl}/LocationGallery/GetGalleryByLocationId?locationId={locationId}";
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при ссылок на фото локаций: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.LocationGallery>>() ?? new List<API.Models.LocationGallery>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetLocationGalleryByIdLocationAsync");
                throw;
            }
        }
    }
}
