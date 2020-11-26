using AuthBase.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthBase.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddAppAuth(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<AuthOptions>(cfg.GetSection(AuthOptions.Security));

            services.AddDbContext<AuthDbContext>(opts => 
                opts.UseSqlServer(cfg.GetConnectionString("Authentication")));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            var authOpts = new AuthOptions();
            cfg.GetSection(AuthOptions.Security).Bind(authOpts);
            
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
                        ValidateLifetime = true,
                        ValidIssuer = authOpts.Issuer,
                        ValidAudience = authOpts.Audience,
                        IssuerSigningKey = authOpts.GetSymmetricSecurityKey()
                    };
                });

            return services;
        }
    }
}