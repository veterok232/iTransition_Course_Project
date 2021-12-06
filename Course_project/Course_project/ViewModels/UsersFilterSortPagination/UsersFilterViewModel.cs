using System;

namespace Course_project.ViewModels.UsersFilterSortPagination
{
    /// <summary>
    /// View model for filtering users
    /// </summary>
    public class UsersFilterViewModel
    {
        /// <summary>
        /// User name filter
        /// </summary>
        public string UserNameFilter { get; set; }

        /// <summary>
        /// User id filter
        /// </summary>
        public string UserIdFilter { get; set; }
    }
}
