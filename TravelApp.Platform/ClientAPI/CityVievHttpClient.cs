namespace TravelApp.Platform.ClientAPI
{
    public class CityViewHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string xPathCity = "https://localhost:7040/api";
        public CityViewHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<API.Models.CityInHomePage>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"{xPathCity}/City/GetCityInHomePage");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.CityInHomePage>>();
        }
    }
}
