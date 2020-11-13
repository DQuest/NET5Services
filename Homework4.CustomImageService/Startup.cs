using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Homework4.CustomImageService.Clients;
using Homework4.CustomImageService.Interfaces;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Refit;

namespace Homework4.CustomImageService
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Homework4.CustomImageService", Version = "v1"});
            });

            var refitSettings = GetRefitSettings();
            services.TryAddTransient(ImplementationFactory(refitSettings, "https://cloud-api.yandex.net"));

            services.AddTransient<ICustomImageService, Services.CustomImageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Homework4.CustomImageService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private RefitSettings GetRefitSettings()
        {
            return new RefitSettings()
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    })
            };
        }
        
        private Func<IServiceProvider, ICustomImageClient> ImplementationFactory(
            RefitSettings refitSettings,
            string uriAddress)
        {
            return _ => RestService.For<ICustomImageClient>(new HttpClient
            {
                BaseAddress = new Uri(uriAddress)
            }, refitSettings);
        }
    }
}