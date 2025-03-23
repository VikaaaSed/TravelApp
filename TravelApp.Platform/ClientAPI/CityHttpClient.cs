namespace TravelApp.Platform.ClientAPI
{
    public class CityHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string xPathCity = "https://localhost:7040/api";
        public CityHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<API.Models.City>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{xPathCity}/City/GetAll");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.City>>();
        }
        public async Task<IEnumerable<API.Models.City>> GetVisibleCityAsync()
        {
            var response = await _httpClient.GetAsync($"{xPathCity}/City/GetVisibleCity");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.City>>();
        }
    }
}
