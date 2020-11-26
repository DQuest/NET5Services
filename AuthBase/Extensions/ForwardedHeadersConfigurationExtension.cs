using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace AuthBase.Extensions
{
    public static class ForwardedHeadersConfigurationExtension
    {
        public static void AddForwardedHeadersConfiguration(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(opts =>
            {
                opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                
                // https://docs.microsoft.com/ru-ru/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1
                // Only loopback proxies are allowed by default.
                // Clear that restriction because forwarders are enabled by explicit configuration.
                opts.KnownNetworks.Clear();
                opts.KnownProxies.Clear();
            });
        }
    }
}