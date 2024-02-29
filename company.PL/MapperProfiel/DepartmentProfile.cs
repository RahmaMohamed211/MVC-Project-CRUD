using AutoMapper;
using Company.DAL.models;
using company.PL.ViewModels;

namespace company.PL.MapperProfiel
{
    public class DepartmentProfile :Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
