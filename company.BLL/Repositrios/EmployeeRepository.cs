using company.BLL.interfaces;
using Company.DAL.Context;
using Company.DAL.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace company.BLL.Repositrios
{
    public class EmployeeRepository : GenericRepository<Employee>, IemployeeRepository
    {
        private readonly CompanyContext dbContext;
        public EmployeeRepository(CompanyContext dbContext) :base(dbContext) //ask clr for creating object from dbcontext
        {
            this.dbContext = dbContext;
        }
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
           
            return dbContext.employees.Where(E=>E.Address.ToLower().Contains(address.ToLower()));

        }

        public IQueryable<Employee> GetEmployeesByName(string name)
        {
            return this.dbContext.employees.Where(E => E.Name.ToLower().Contains(name.ToLower())).Include(E=>E.Department);
        }
    }
}
