using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Disertatie.Core
{
    public static class QuerableExtensions
    {
        public static PageList<TEntity> ToPageList<TEntity>(this IQueryable<TEntity> queryable, int pageIndex, int pageSize) {
            var count = queryable.Count();
            var results = queryable.Skip(pageIndex * pageSize).Take(pageSize);
            return new PageList<TEntity>(pageIndex, count, pageSize, results);
        }

        public static PageList<TEntity> ToPageList<TEntity>(this IQueryable<TEntity> queryable, int pageIndex) {
            return queryable.ToPageList(pageIndex, PageList<TEntity>.DefaultPageSize);
        }

        public static PageList<TEntity> ToPageList<TEntity>(this IQueryable<TEntity> queryable) {
            return queryable.ToPageList(1);
        }


        public static IQueryable<TEntity> WhereIn<TEntity, TValue>
          (
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, TValue>> selector,
            IEnumerable<TValue> collection
          )
        {
            if (selector == null) throw new ArgumentNullException("selector");
            if (collection == null) throw new ArgumentNullException("collection");
            ParameterExpression p = selector.Parameters.Single();

            if (!collection.Any()) return query;

            IEnumerable<System.Linq.Expressions.Expression> equals = collection.Select(value =>
               (System.Linq.Expressions.Expression)System.Linq.Expressions.Expression.Equal(selector.Body,
                    System.Linq.Expressions.Expression.Constant(value, typeof(TValue))));

            System.Linq.Expressions.Expression body = equals.Aggregate((accumulate, equal) =>
                System.Linq.Expressions.Expression.Or(accumulate, equal));

            return query.Where(System.Linq.Expressions.Expression.Lambda<Func<TEntity, bool>>(body, p));
        }
    }
}
