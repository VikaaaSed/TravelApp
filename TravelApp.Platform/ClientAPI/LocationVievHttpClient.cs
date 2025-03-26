namespace TravelApp.Platform.ClientAPI
{
    public class LocationViewHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string xPathCity = "https://localhost:7040/api";
        public LocationViewHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<API.Models.LocationInCity>> GetLocationInCitiesByCityIdAsync(int cityId)
        {
            string url = $"{xPathCity}/Location/GetLocationsByCityId";

            if (cityId != 0)
                url += $"?cityId={cityId}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.LocationInCity>>();
        }
        public async Task<API.Models.LocationInHomePage> GetLocationByPageNameAsync(string pageName)
        {
            string url = $"{xPathCity}/Location/GetLocationByPageName";

            if (pageName != null)
                url += $"?pageName={pageName}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<API.Models.LocationInHomePage>();
        }
    }
}
