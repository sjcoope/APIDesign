using Microsoft.EntityFrameworkCore;
using SJCNet.APIDesign.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SJCNet.APIDesign.Data.Repository
{
    public abstract class EFRepository<T> : IRepository<T> where T : class, IEntity
    {
        public EFRepository(DataContext context)
        {
            this.Context = context;
        }
        
        protected abstract DbSet<T> DbSet { get; }

        protected DataContext Context { get; set; }

        public IQueryable<T> Get()
        {
            return this.DbSet.AsQueryable();
        }

        public T Get(int id)
        {
            return this.DbSet.SingleOrDefault(i => i.Id == id);
        }

        public bool Add(T entity)
        {
            this.DbSet.Add(entity);
            return Context.SaveChanges() != 0;
        }

        public bool Update(T entity)
        {
            this.DbSet.Attach(entity);
            this.DbSet.Update(entity);
            return Context.SaveChanges() != 0;
        }

        public bool Delete(T entity)
        {
            this.DbSet.Attach(entity);
            this.DbSet.Remove(entity);
            return Context.SaveChanges() != 0;
        }

        public bool Delete(int id)
        {
            var match = this.DbSet.SingleOrDefault(i => i.Id == id);
            if(match != null)
            {
                this.DbSet.Remove(match);
                return Context.SaveChanges() != 0;
            }

            return false;
        }
    }
}
