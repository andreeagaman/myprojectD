using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;

namespace Disertatie.Core.Repositories
{
    public class Repository
    {
        private readonly ISession _session;
        public ISession Session {
            get { return _session; }
        }

        public Repository(ISession session) {
            if (session == null)
                throw new ArgumentNullException("session");

            this._session = session;
        }

        public Repository() : this(Disertatie.Core.SessionManager.GetCurrentSession()) { }

        public TEntity Get<TEntity>(object id) {
            return _session.Get<TEntity>(id);
        }

        public TEntity Load<TEntity>(object id) {
            return _session.Load<TEntity>(id);
        }

        public object Save(object entity) {
            return _session.Save(entity);
        }

        public void Update(object entity) {
            _session.Update(entity);
        }

        public void SaveOrUpdate(object entity) {
            _session.SaveOrUpdate(entity);
        }

        public void Delete(object entity) {
            _session.Delete(entity);
        }

        public IQueryable<TEntity> QueryAll<TEntity>() {
            return _session.Query<TEntity>();
        }
    }
}
