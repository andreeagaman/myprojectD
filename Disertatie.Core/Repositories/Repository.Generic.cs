using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace Disertatie.Core.Repositories
{
    public class Repository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity: class
    {
        private readonly ISession _session;

        public ISession Session {
            get {
                return _session;
            }
        }

        public Repository(ISession session) {
            if (session == null)
                throw new ArgumentNullException("session");

            this._session = session;
        }

        public Repository() : this(SessionManager.GetCurrentSession()) { }

        #region IRepository<TKey,TEntity> Members

        public virtual TEntity Get(TKey id) {
            return _session.Get<TEntity>(id);
        }

        public virtual TEntity Load(TKey id) {
            return _session.Load<TEntity>(id);
        }

        public virtual TKey Save(TEntity entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return (TKey)_session.Save(entity);
        }

        public virtual void Update(TEntity entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _session.Update(entity);
        }

        public void SaveOrUpdate(TEntity entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _session.SaveOrUpdate(entity);
        }

        public void Delete(TEntity entity) {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _session.Delete(entity);            
        }

        public void Delete(TKey key) {
            _session.Delete(_session.Load<TEntity>(key));
        }

        public IQueryable<TEntity> QueryAll() {
            return _session.Query<TEntity>();
        }

        #endregion

        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate) {
            return _session.Query<TEntity>().Where(predicate).SingleOrDefault();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate) {
            return _session.Query<TEntity>().Where(predicate);
        }

        public static void LoadProxy(object proxy) {
            NHibernateUtil.Initialize(proxy);
        }
    }
}
