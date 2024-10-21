using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Linq.Expressions;
using Disertatie.Core.Models;

namespace Disertatie.Core.Repositories
{
    public abstract class RepositoryBase<TKey, TEntity> : Repository<TKey, TEntity>
        where TEntity: class
    {
        public RepositoryBase(ISession session) : base(session) { }
        public RepositoryBase() : base() { }

        #region Abstract Members

        public abstract Expression<Func<TEntity, object>> DefaultOrderBy { get; }

        #endregion

        public PageList<TEntity> GetPageList(int pageIndex, int pageSize) {
            return this.QueryAll().OrderBy(DefaultOrderBy).ToPageList(pageIndex, pageSize);
        }

        public PageList<TEntity> GetPageList(int pageIndex) {
            return this.QueryAll().OrderBy(DefaultOrderBy).ToPageList(pageIndex);
        }
       
    }
}
