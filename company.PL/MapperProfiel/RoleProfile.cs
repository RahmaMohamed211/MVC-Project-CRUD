using AutoMapper;
using company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace company.PL.MapperProfiel
{
    public class RoleProfile :Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel,IdentityRole>()
                .ForMember(d=>d.Name,o=>o.MapFrom(s=>s.RoleName)).ReverseMap();
        }
    }
}
