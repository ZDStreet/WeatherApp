using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherData?> GetWeatherAsync(string location)
        {
            try
            {
                var apiKey = _configuration["ApiKeys:OpenWeatherMap"];
                var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={location}&appid={apiKey}&units=metric");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<WeatherData>();
            }
            catch (HttpRequestException)
            {
                // Handle error (e.g., log it, return null, etc.)
                return null;
            }
        }

        public async Task<List<string>> GetLocationSuggestions(string term)
        {
            try
            {
                if (string.IsNullOrEmpty(term))
                    return new List<string>();

                var username = _configuration["ApiKeys:GeoNames:Username"];
                var encodedTerm = Uri.EscapeDataString(term);
                var response = await _httpClient.GetAsync(
                    $"http://api.geonames.org/searchJSON?q={encodedTerm}&maxRows=5&cities=cities1000&username={username}");
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode}");
                    return new List<string>();
                }

                var result = await response.Content.ReadFromJsonAsync<GeoNamesResponse>();
                
                if (result?.Geonames == null)
                    return new List<string>();

                return result.Geonames
                    .Select(city => city.Name)
                    .Distinct()
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetLocationSuggestions: {ex.Message}");
                return new List<string>();
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

    public class GeoNamesResponse
    {
        [JsonPropertyName("geonames")]
        public List<GeoName> Geonames { get; set; } = new();
    }

    public class GeoName
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("countryName")]
        public string CountryName { get; set; } = string.Empty;
    }
}