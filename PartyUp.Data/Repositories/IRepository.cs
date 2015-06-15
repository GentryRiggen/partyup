using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<IQueryable<T>> GetAllAsync();

        T Find(int id);
        Task<T> FindAsync(int id);

        T Find(int? id);
        Task<T> FindAsync(int? id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(int id);

        bool Exists(int id);
        Task<bool> ExistsAsync(int id);
    }
}
