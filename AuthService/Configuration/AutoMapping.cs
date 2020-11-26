using AuthBase.Models;
using AuthService.Models.SignUp;
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