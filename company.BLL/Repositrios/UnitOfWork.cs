using company.BLL.interfaces;
using Company.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.BLL.Repositrios
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly CompanyContext dbContext;

        public IemployeeRepository EmployeeRepository { get ; set; }
        public IDepartmentRepository DepartmentRepository { get ; set ; }

        public UnitOfWork(CompanyContext dbContext) //ask for object from dbcontext
        {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
            this.dbContext = dbContext;
        }

        public  async Task<int> Compelete()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Dispose()
          => dbContext.Dispose();
        
    }
}
