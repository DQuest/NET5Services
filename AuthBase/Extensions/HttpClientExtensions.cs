using System;
using System.Net.Http;
using AuthBase.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

namespace AuthBase.Extensions
{
    public static class HttpClientExtensions
    {
        private const string DefaultUrlKey = "Api:Url";

        public static IServiceCollection AddApiClient<T>(
            this IServiceCollection services,
            IConfiguration cfg,
            RefitSettings refitSettings,
            string cfgUrlKey = null)
            where T : class
        {
            services.AddHttpClientHandlers();
            services.Configure<ApiSettings>(cfg.GetSection("Api"));
            SetupClient<T>(services, cfg, refitSettings, cfgUrlKey);

            return services;
        }

        internal static IServiceCollection AddHttpClientHandlers(this IServiceCollection services)
        {
            services.TryAddTransient<AuthHttpClientHandler>();
            return services;
        }

        private static HttpMessageHandler BuildHandler(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<AuthHttpClientHandler>();
        }
        
        private static void SetupClient<T>(
            IServiceCollection services,
            IConfiguration cfg,
            RefitSettings refitSettings,
            string cfgUrlKey)
            where T : class
        {
            var baseUrl = cfg[cfgUrlKey ?? DefaultUrlKey];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new NullReferenceException($"BaseUrl is empty for {nameof(T)}");
            }

            services.TryAddTransient<T>(provider =>
            {
                // Сборка клиента вручную, проблема dotnet.runtime
                // https://github.com/dotnet/runtime/issues/36574

                var httpMessageHandler = provider.BuildHandler();
                var client = new HttpClient(httpMessageHandler)
                {
                    BaseAddress = new Uri(baseUrl.TrimEnd('/'))
                };

                return RestService.For<T>(client, refitSettings);
            });
        }
    }
}