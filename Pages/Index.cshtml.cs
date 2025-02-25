using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeatherApp.Services;

namespace WeatherApp.Pages
{
    [IgnoreAntiforgeryToken]
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

        public WeatherData? WeatherData { get; set; } 

        public async Task OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Location))
            {
                WeatherData = await _weatherService.GetWeatherAsync(Location);
                if (WeatherData == null)
                {
                    ModelState.AddModelError("Location", "Please enter a valid city name.");
                }
            }
        }

        public async Task<JsonResult> OnGetLocationSuggestions(string term)
        {
            if (string.IsNullOrEmpty(term))
                return new JsonResult(new List<string>());
        
            var suggestions = await _weatherService.GetLocationSuggestions(term);
            return new JsonResult(suggestions);
        }
    }
}