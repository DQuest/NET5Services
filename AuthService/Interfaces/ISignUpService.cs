using System.Threading.Tasks;
using AuthService.Models.SignUp;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Interfaces
{
    public interface ISignUpService
    {
        Task<ObjectResult> SignUp(SignUpModel signUpModel);
    }
}