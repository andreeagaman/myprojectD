using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disertatie.Core
{
    public interface IPagedList
    {
        int PageIndex { get; }
        int PageCount { get; }
        int PageSize { get; }
        int TotalCount { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
