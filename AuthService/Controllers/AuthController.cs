using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AuthService.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            // todo вынести в сервис, замапить пользователя через autoMapper
            var userExists = await _userManager.FindByNameAsync(signUpModel.Username);

            if (userExists != null)
            {
                return new ConflictObjectResult(new SignUpResponse
                {
                    Status = "Error",
                    Message = "User already exists"
                });
            }

            var user = new User
            {
                Email = signUpModel.Email,
                UserName = signUpModel.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new SignUpResponse
                {
                    Status = "Error",
                    Message = "User creation failed"
                });
            }

            return new OkObjectResult(new SignUpResponse
            {
                Status = "Success",
                Message = "User created successfully"
            });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> SignUp(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                return new UnauthorizedObjectResult(new LoginResponse
                {
                    Username = loginModel.Username
                });
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            claims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authLoginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:Secret"]));

            var token = new JwtSecurityToken(
                _configuration["Security:Issuer"],
                _configuration["Security:Audience"],
                expires: DateTime.Now.AddHours(24),
                claims: claims,
                signingCredentials: new SigningCredentials(authLoginKey, SecurityAlgorithms.Sha256));
            
            return new OkObjectResult(new LoginResponse
            {
                Username = loginModel.Username,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            })
        }
    }
}