using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using MVCMovie.Models;
using System.Linq.Expressions;

namespace MVCMovie.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private RecruitingSiteDBContext context;
        private DbSet<TEntity> dbSet;
        private bool disposed = false;


        public Repository(RecruitingSiteDBContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.ToList<TEntity>();
        }

        public TEntity GetByID(int id)
        {
            TEntity entity = dbSet.Find(id);
            return entity;
        }

        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public TEntity InsertAndReturn(TEntity entity)
        {
            TEntity ret_entity = dbSet.Add(entity);
            context.SaveChanges();
            return ret_entity;
        }

        public void Delete(int id)
        {
            TEntity entity = dbSet.Find(id);
            if (entity == null)
            {
                return;
            }
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}