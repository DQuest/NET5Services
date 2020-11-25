using System;
using System.Net.Http;
using System.Text;
using AutoMapper;
using ImageService.Clients;
using ImageService.Configuration;
using ImageService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Refit;

namespace ImageService
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ImageService", Version = "v1"});
            });
            
            services.AddTransient<IImageService, Services.ImageService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            SetupRefit(services);
            AddAutoMapper(services);
            AddAuthentication(services);
            AddDbContext(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
        
        private void SetupRefit(IServiceCollection services)
        {
            var refitSettings = GetRefitSettings();
            services.TryAddTransient(
                ImplementationFactory<IYandexDiskImageClient>(refitSettings, "https://cloud-api.yandex.net"));
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
        
        private void AddAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Configuration["Security:Issuer"],
                        ValidAudience = Configuration["Security:Audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Security:Secret"]))
                    };
                });
        }
        
        private void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new AutoMapping()); });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private void AddDbContext(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Image");
            services.AddDbContext<ImageContext>(options => options.UseSqlServer(connectionString));
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