using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthService.Interfaces;
using AuthService.Models.Login;
using AuthService.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public LoginService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentException(nameof(configuration));
        }

        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                return new NotFoundObjectResult(new LoginResponse
                {
                    Username = loginModel.Username
                });
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = GenerateJwtSecurityToken(user, userRoles);

            return new OkObjectResult(new LoginResponse
            {
                Username = loginModel.Username,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        private JwtSecurityToken GenerateJwtSecurityToken(User user, IList<string> userRoles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authLoginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:Secret"]));

            return new JwtSecurityToken(
                _configuration["Security:Issuer"],
                _configuration["Security:Audience"],
                expires: DateTime.Now.AddHours(24),
                claims: claims,
                signingCredentials: new SigningCredentials(authLoginKey, SecurityAlgorithms.Sha256));
        }
    }
}