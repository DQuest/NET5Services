using AuthBase.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductService.Configuration;
using ProductService.Extensions;
using ProductService.Interfaces;

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
            services.AddAppAuth(Configuration);
            services.AddServiceClients(Configuration);
            services.AddForwardedHeadersConfiguration();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "ProductService", Version = "v1"});
            });

            services.AddScoped<IProductService, ProductService.Services.ProductService>();

            AddDbContext(services);
            AddAutoMapper(services);
            AddNewtonsoftJson(services);
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

        private void AddNewtonsoftJson(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            });
        }
        
        private void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            var mapper = new MapperConfiguration(
                    config => { config.AddProfile(new AutoMapping()); })
                .CreateMapper();

            services.AddSingleton(mapper);
        }

        private void AddDbContext(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<ProductContext>(opts =>
                    opts.UseNpgsql(Configuration.GetConnectionString("Product")));
        }
    }
}