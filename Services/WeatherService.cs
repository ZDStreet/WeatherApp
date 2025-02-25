using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherData> GetWeatherAsync(string location)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={location}&appid=665ccddd0e42978e71bc9830ae72dbc9&units=metric");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<WeatherData>();
            }
            catch (HttpRequestException)
            {
                // Handle error (e.g., log it, return null, etc.)
                return null;
            }
        }
    }

    public class WeatherData
    {
        public Main? Main { get; set; }
        public string? Name { get; set; }
    }

    public class Main
    {
        public float Temp { get; set; }
    }
}