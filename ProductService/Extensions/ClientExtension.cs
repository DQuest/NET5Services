using AuthBase.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProductService.Clients;
using Refit;

namespace ProductService.Extensions
{
    /// <summary>
    /// ToDo: вынести в AuthBase
    /// </summary>
    public static class ClientExtension
    {
        private const string ImageServiceUrlKey = "Api:ImageService";
        private const string PriceServiceUrlKey = "Api:PriceService";

        public static IServiceCollection AddServiceClients(this IServiceCollection services, IConfiguration cfg)
        {
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    })
            };

            services.AddApiClient<IImageClient>(cfg, refitSettings, ImageServiceUrlKey);
            services.AddApiClient<IPriceClient>(cfg, refitSettings, PriceServiceUrlKey);

            return services;
        }
    }
}