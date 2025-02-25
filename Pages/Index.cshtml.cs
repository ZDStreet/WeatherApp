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
                if (WeatherData == null)
                {
                    // Handle invalid city name
                    ModelState.AddModelError("Location", "Please enter a valid city name.");
                }
            }
        }

        [IgnoreAntiforgeryToken]
        public async Task<JsonResult> OnGetLocationSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
                return new JsonResult(new List<string>());
        
            var suggestions = await _weatherService.GetLocationSuggestions(term);
            return new JsonResult(suggestions);
        }
    }
}