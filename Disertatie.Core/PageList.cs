using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core
{
    public class PageList<TEntity> : List<TEntity>, IPagedList
    {
        private int _numLinks = 2; 
        public const int DefaultPageSize = 25;

        public int PageIndex { get; private set; }
        public int PageCount { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public int NumLinks { get { return _numLinks; } set { _numLinks = value; } }
        public bool HasPreviousPage {
            get { return this.PageIndex > 0; }
        }
        public bool HasNextPage {
            get { return this.PageIndex < this.PageCount - 1; }
        }

        public PageList(int pageIndex, int totalCount, int pageSize, IEnumerable<TEntity> results) {
            
            if (pageIndex < 0)
                throw new ArgumentOutOfRangeException("pageIndex");

            if (totalCount < 0)
                throw new ArgumentOutOfRangeException("pageCount");

            if (results == null)
                throw new ArgumentNullException("results");

            int pageResult = 0;
            for (int counter = 1; pageResult < this.TotalCount; counter++)
            {
                pageResult = counter * this.PageSize;
                this.TotalCount = counter;
            }
            try
            {
                this.PageIndex = pageIndex;
                this.TotalCount = totalCount;
                this.PageSize = pageSize;
                this.PageCount = (this.TotalCount / this.PageSize) + 1;
                this.AddRange(results);
            }
            catch
            {
            }
          // this.AddRange(results.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public PageList(int pageIndex, int totalCount, IEnumerable<TEntity> results) :
            this(pageIndex, totalCount, DefaultPageSize, results) { }   



    }
}
