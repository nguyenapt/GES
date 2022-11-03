using GES.Common.Exceptions;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GES.Inside.Data.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Method will execute the query and return the result based on current page size and page index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="logger"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <exception cref="GesServiceException">The exception can be throw</exception>
        /// <returns></returns>
        public static PaginatedResults<T> ToPagedList<T>(this IEnumerable<T> query, IGesLogger logger, int pageIndex, int pageSize, string message = "")
        {
            var totalResults = query.Execute<int, System.Data.DataException, GesServiceException>(logger, query.Count);
            pageSize = pageSize > 0 ? pageSize : totalResults;
            var items = query.Execute<IList<T>, System.Data.DataException, GesServiceException>(logger, () => query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
            
            return new PaginatedResults<T>(items, totalResults, pageSize, pageIndex, message);
        }
    }
}
