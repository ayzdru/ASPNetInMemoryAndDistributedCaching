using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace AspNetInMemoryAndDistributedCaching.Pages
{
    public class MemoryCacheModel : PageModel
    {
        private readonly ILogger<MemoryCacheModel> _logger;
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheModel(ILogger<MemoryCacheModel> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }
        public WeatherForecast WeatherForecast { get; set; }
        public void OnGet()
        {
            _memoryCache.TryGetValue<WeatherForecast>("WeatherForecast", out WeatherForecast weatherForecast);
            if (weatherForecast !=null)
            {
                WeatherForecast = weatherForecast;
            }
        }
    }
}
