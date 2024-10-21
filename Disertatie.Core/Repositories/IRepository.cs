using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core.Repositories
{
    public interface IRepository<TKey, TEntity> where TEntity: class
    {
        TEntity Get(TKey id);
        TEntity Load(TKey id);

        TKey Save(TEntity entity);
        void Update(TEntity entity);
        void SaveOrUpdate(TEntity entity);

        void Delete(TEntity entity);
        void Delete(TKey key);

        IQueryable<TEntity> QueryAll();
    }

    public interface IRepository<TEntity> : IRepository<object, TEntity> where TEntity : class
    {

    }

    public interface IRepository : IRepository<object>
    {

    }
}
