using System;

namespace Course_project.ViewModels.UsersFilterSortPagination
{
    /// <summary>
    /// View model for users paging
    /// </summary>
    public class UsersPageViewModel
    {
        /// <summary>
        /// Current page
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Total pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="count"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public UsersPageViewModel(int count, int pageNumber, int pageSize)
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
