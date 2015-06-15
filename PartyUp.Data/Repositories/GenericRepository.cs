using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> DbSet { get; set; }
        protected ApplicationDbContext context { get; set; }
        public GenericRepository(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required!", "context was null");
            }
            else
            {
                this.context = context;
                this.DbSet = this.context.Set<T>();
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            return this.DbSet;
        }

        public virtual async Task<IQueryable<T>> GetAllAsync()
        {
            var all = await this.DbSet.ToListAsync();
            return all.AsQueryable();
        }

        public virtual T Find(int id)
        {
            return this.DbSet.Find(id);
        }

        public virtual async Task<T> FindAsync(int id)
        {
            return await this.DbSet.FindAsync(id);
        }

        public virtual T Find(int? id)
        {
            return this.DbSet.Find(id);
        }

        public virtual async Task<T> FindAsync(int? id)
        {
            return await this.DbSet.FindAsync(id);
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry entry = this.context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry entry = this.context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = this.Find(id);
            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual bool Exists(int id)
        {
            return this.Find(id) != null;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await this.FindAsync(id) != null;
        }
    }
}
