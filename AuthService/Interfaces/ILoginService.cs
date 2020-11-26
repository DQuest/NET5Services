using System.Threading.Tasks;
using AuthService.Models.Login;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Interfaces
{
    public interface ILoginService
    {
        Task<ObjectResult> Login(LoginModel loginModel);
    }
}