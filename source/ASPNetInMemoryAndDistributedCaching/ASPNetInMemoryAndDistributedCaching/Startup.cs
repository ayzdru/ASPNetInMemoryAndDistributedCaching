using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Alachisoft.NCache.Caching.Distributed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASPNetInMemoryAndDistributedCaching
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //Distributed Memory Cache
            services.AddDistributedMemoryCache();

            //Distributed SQL Server Cache
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString =
            //        Configuration["DistributedSQLServerCache"];
            //    options.SchemaName = "dbo";
            //    options.TableName = "TestCache";
            //});

            //Distributed Redis Cache
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = "localhost";
            //    options.InstanceName = "SampleInstance";
            //});

            //Distributed NCache Cache
            //services.AddNCacheDistributedCache(configuration =>
            //{
            //    configuration.CacheName = "demoClusteredCache";
            //    configuration.EnableLogs = true;
            //    configuration.ExceptionsEnabled = true;
            //});
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IDistributedCache cache, IMemoryCache memoryCache)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                var weatherForecast = new WeatherForecast
                {
                    Date = DateTime.Now,
                    Summary = "Freezing",
                    TemperatureCelsius = -20
                };
                memoryCache.Set("WeatherForecast", weatherForecast);
                var jsonString = JsonSerializer.Serialize(weatherForecast);
                cache.SetString("WeatherForecast", jsonString, options);
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
