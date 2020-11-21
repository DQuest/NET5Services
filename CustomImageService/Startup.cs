using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CustomImageService.Clients;
using CustomImageService.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Refit;

namespace CustomImageService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CustomImageService", Version = "v1"});
            });

            var refitSettings = GetRefitSettings();
            services.TryAddTransient(ImplementationFactory<IYandexDriveImageClient>(refitSettings, "https://cloud-api.yandex.net"));
            services.TryAddTransient(ImplementationFactory<IImageDbClient>(refitSettings, "https://localhost:5003"));

            services.AddTransient<ICustomImageService, Services.CustomImageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomImageService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private RefitSettings GetRefitSettings()
        {
            return new()
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    })
            };
        }
        
        private Func<IServiceProvider, T> ImplementationFactory<T>(
            RefitSettings refitSettings,
            string uriAddress)
        {
            return _ => RestService.For<T>(new HttpClient
            {
                BaseAddress = new Uri(uriAddress)
            }, refitSettings);
        }
    }
}