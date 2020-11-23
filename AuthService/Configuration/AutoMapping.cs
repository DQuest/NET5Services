using AuthService.Models.SignUp;
using AuthService.Models.User;
using AutoMapper;

namespace AuthService.Configuration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<SignUpModel, User>();
            CreateMap<User, SignUpModel>();
        }
    }
}