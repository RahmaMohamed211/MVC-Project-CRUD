using Company.DAL.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.BLL.interfaces
{
    public interface IGenericRepository<T> 
    {//Task

       Task< IEnumerable<T>> GetAll();

        Task<T> Get(int id);

        Task Add(T entity);

        void Update(T entity);

        void Delete(T entity   );
    }
}
