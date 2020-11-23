using System;
using System.Threading.Tasks;
using AuthService.Interfaces;
using AuthService.Models.Login;
using AuthService.Models.SignUp;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISignUpService _signUpService;
        private readonly ILoginService _loginService;
        private readonly IRestorePasswordService _restorePasswordService;

        public AuthController(
            ISignUpService signUpService,
            ILoginService loginService,
            IRestorePasswordService restorePasswordService)
        {
            _signUpService = signUpService ?? throw new ArgumentException(nameof(signUpService));
            _loginService = loginService ?? throw new ArgumentException(nameof(loginService));
            _restorePasswordService = restorePasswordService ?? throw new ArgumentException(nameof(restorePasswordService));;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            return await _signUpService.SignUp(signUpModel);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            return await _loginService.Login(loginModel);
        }

        // [HttpPost("restore")]
        // public async Task<IActionResult> RestorePassword(RestorePasswordModel restorePasswordModel)
        // {
        //     return await _restorePasswordService.RestorePassword(restorePasswordModel);
        // }
    }
}