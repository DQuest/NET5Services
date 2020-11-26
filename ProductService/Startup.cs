using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductService.Clients;
using ProductService.Interfaces;
using Refit;

namespace ProductService
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
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ProductService", Version = "v1"});
            });

            services.AddScoped<IProductService, ProductService.Services.ProductService>();

            services.AddEntityFrameworkNpgsql().AddDbContext<ProductContext>();
            AddAuthentication(services);
            AddNewtonsoftJson(services);
            SetupRefit(services);
        }

        private Func<IServiceProvider, T> ImplementationFactory<T>(RefitSettings refitSettings, string uri)
        {
            return _ => RestService.For<T>(new HttpClient
            {
                BaseAddress = new Uri(uri)
            }, refitSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService v1"));

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthorization();
                
                app.UseAuthentication();

                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            }
        }
        
        private void SetupRefit(IServiceCollection services)
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

            services.TryAddTransient(ImplementationFactory<IImageClient>(refitSettings, "https://localhost:5003"));
            services.TryAddTransient(ImplementationFactory<IPriceClient>(refitSettings, "https://localhost:5005"));
        }

        private void AddNewtonsoftJson(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            });
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
    }
}