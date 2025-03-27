namespace TravelApp.Platform.ClientAPI
{
    public class LocationGalleryHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string xPathCity = "https://localhost:7040/api";
        public LocationGalleryHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<API.Models.LocationGallery>> GetLocationGalleryByLocationIdAsync(int locationId)
        {
            string url = $"{xPathCity}/LocationGallery/GetGalleryByLocationId";

            if (locationId != 0)
                url += $"?locationId={locationId}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.LocationGallery>>();
        }
    }
}
