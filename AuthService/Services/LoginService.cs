using System;
using System.Threading.Tasks;
using AuthBase;
using AuthBase.Models;
using AuthService.Interfaces;
using AuthService.Models.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppSecurity _appSecurity;

        public LoginService(UserManager<User> userManager, AppSecurity appSecurity)
        {
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _appSecurity = appSecurity ?? throw new ArgumentException(nameof(appSecurity));
        }

        public async Task<ObjectResult> Login(LoginModel loginModel)
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

            var token = _appSecurity.GetToken(user.Id, DateTime.Now, userRoles);

            return new OkObjectResult(new LoginResponse
            {
                Username = loginModel.Username,
                Token = token
            });
        }
    }
}