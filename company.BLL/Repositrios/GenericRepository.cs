using company.BLL.interfaces;
using Company.DAL.Context;
using Company.DAL.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.BLL.Repositrios
{
    public class GenericRepository<T> : IGenericRepository<T>  
        where T : class
    {
        private protected readonly CompanyContext dbContext;
        public GenericRepository(CompanyContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Add(T entity)
        
       =>   await dbContext.Set<T>().AddAsync(entity); //Ef core 3.1 feature
          //  return this.dbContext.SaveChanges();
        
        public void Delete(T entity)
        {
            this.dbContext.Set<T>().Remove(entity);
           // return this.dbContext.SaveChanges();
        }

       

        public async Task<T> Get(int id)
       =>  await dbContext.Set<T>().FindAsync(id);

        public  async Task<IEnumerable<T>> GetAll()
        { 
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>) await dbContext.employees.Include(E => E.Department).ToListAsync();
            else
                return  await dbContext.Set<T>().ToListAsync();
        }



        public void Update(T entity)
        {
          this.dbContext.Set<T>().Update(entity);
          //  return this.dbContext.SaveChanges();
        }

      

       

       
    }
}
