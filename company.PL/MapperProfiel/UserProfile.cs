using AutoMapper;
using company.PL.ViewModels;
using Company.DAL.models;

namespace company.PL.MapperProfiel
{
    public class UserProfile : Profile
    {
        public UserProfile() {
        
        CreateMap<ApplicationUser,UserViewModel>().ReverseMap();
        }

    }
}
