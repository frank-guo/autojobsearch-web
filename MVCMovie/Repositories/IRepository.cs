using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MVCMovie.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "");
        TEntity GetByID(int id);
        void Insert(TEntity entity);
        TEntity InsertAndReturn(TEntity entity);
        void Delete(int id);
        //void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}
