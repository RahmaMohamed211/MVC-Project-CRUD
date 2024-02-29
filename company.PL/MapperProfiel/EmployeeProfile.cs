using AutoMapper;
using company.PL.ViewModels;
using Company.DAL.models;

namespace company.PL.MapperProfiel
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
               
        }
    }
}
