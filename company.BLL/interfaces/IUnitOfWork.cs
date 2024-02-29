using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.BLL.interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IemployeeRepository EmployeeRepository { get; set; }
        
        IDepartmentRepository DepartmentRepository { get; set; }

        Task<int> Compelete();

        
    }
}
