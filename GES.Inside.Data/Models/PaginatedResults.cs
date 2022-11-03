using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class PaginatedResults<TResultType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedResults{TResultType}" /> class.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="totalResults">The total results.</param>
        /// <param name="pageIndex"></param>
        public PaginatedResults(IEnumerable<TResultType> results, int totalResults, int pageSize, int pageIndex, string message = "")
        {
            Results = results;
            TotalResults = totalResults;
            PageSize = pageSize;
            PageIndex = pageIndex;
            Message = message;
        }

        /// <summary>
        /// Gets the page size
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public IEnumerable<TResultType> Results { get; }

        /// <summary>
        /// Gets the total results.
        /// </summary>
        /// <value>
        /// The total results.
        /// </value>
        public int TotalResults { get; }

        /// <summary>
        /// Gets number of pages
        /// </summary>
        /// <value>
        /// The number of pages.
        /// </value>
        public int TotalPages => (int)Math.Ceiling((float)TotalResults / PageSize);

        /// <summary>
        /// Get the current page index
        /// </summary>
        public int PageIndex { get; }

        public string Message { get; }

    }
}
