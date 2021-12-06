using System;

namespace Course_project.ViewModels.ReviewsFilterSortPagination
{
    /// <summary>
    /// View model for paging
    /// </summary>
    public class PageViewModel
    {
        /// <summary>
        /// Current page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Total page number
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        /// <summary>
        /// Has previous page
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        /// <summary>
        /// Has next page
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }
    }
}
