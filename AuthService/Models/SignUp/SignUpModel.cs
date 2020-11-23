using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.SignUp
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        // public List<IdentityRole> Roles { get; set; }
    }
}