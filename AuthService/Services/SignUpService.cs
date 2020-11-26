using System;
using System.Linq;
using System.Threading.Tasks;
using AuthBase.Models;
using AuthService.Interfaces;
using AuthService.Models.SignUp;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public SignUpService(
            UserManager<User> userManager, 
            IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        
        public async Task<ObjectResult> SignUp(SignUpModel signUpModel)
        {
            var userExists = await _userManager.FindByNameAsync(signUpModel.Username);

            if (userExists != null)
            {
                return new ConflictObjectResult(new SignUpResponse
                {
                    Status = "Error",
                    Message = "User already exists!"
                });
            }

            var user = _mapper.Map<User>(signUpModel);

            var result = await _userManager.CreateAsync(user, signUpModel.Password);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(new SignUpResponse
                {
                    Status = "Error",
                    Message = "User creation failed! " + $"{string.Join(" ", result.Errors.Select(x => x.Description))}"
                });
            }

            return new OkObjectResult(new SignUpResponse
            {
                Status = "Success",
                Message = "User created successfully!"
            });
        }
    }
}