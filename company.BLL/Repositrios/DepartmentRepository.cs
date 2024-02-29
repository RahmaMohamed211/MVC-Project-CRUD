using company.BLL.interfaces;
using Company.DAL.Context;
using Company.DAL.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.BLL.Repositrios
{
    public class DepartmentRepository :GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(CompanyContext dbcontext):base(dbcontext)
        {
            
        }
    }
}
