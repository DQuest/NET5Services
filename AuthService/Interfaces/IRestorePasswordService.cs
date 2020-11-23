using System.Threading.Tasks;
using AuthService.Models.RestorePassword;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Interfaces
{
    public interface IRestorePasswordService
    {
        Task<IActionResult> RestorePassword(RestorePasswordModel restorePasswordModel);
    }
}