using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherApp.Services;

namespace WeatherApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly WeatherService _weatherService;

        public IndexModel(ILogger<IndexModel> logger, WeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [BindProperty]
        public required string Location { get; set; }

        public WeatherData? WeatherData { get; set; } // Make WeatherData nullable

        public async Task OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Location))
            {
                WeatherData = await _weatherService.GetWeatherAsync(Location);
            }
        }
    }
}