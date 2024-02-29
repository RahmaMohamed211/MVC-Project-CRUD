using Company.DAL.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.BLL.interfaces
{
    public interface IemployeeRepository : IGenericRepository<Employee>
    {
    public IQueryable<Employee> GetEmployeesByAddress(string address);

     public IQueryable<Employee> GetEmployeesByName(string name);

    }
}
