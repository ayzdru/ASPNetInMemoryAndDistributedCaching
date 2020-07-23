using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace AspNetInMemoryAndDistributedCaching.Pages
{
    public class DistributedCacheModel : PageModel
    {
        private readonly ILogger<DistributedCacheModel> _logger;
        private readonly IDistributedCache _distributedCache;
        public DistributedCacheModel(ILogger<DistributedCacheModel> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public WeatherForecast WeatherForecast { get; set; }
        public async Task OnGetAsync()
        {
            var weatherForecastJsonString = await _distributedCache.GetStringAsync("WeatherForecast");

            if (weatherForecastJsonString != null)
            {
                WeatherForecast = JsonSerializer.Deserialize<WeatherForecast>(weatherForecastJsonString);
            }
        }
    }
}
