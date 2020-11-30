using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceService.Models;

namespace PriceService.Extensions
{
    public static class PriceDbOptionsExtensions
    {
        public static void AddPriceDbOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PriceDbOptions>(options =>
                options.ConnectionString = configuration.GetConnectionString("Price"));
        }
    }
}