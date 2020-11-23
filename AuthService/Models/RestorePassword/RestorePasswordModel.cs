using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.RestorePassword
{
    public class RestorePasswordModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Password is required")]
        public string Email { get; set; }
    }
}