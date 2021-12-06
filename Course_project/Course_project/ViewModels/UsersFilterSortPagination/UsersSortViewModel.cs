using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Course_project.ViewModels.UsersFilterSortPagination
{
    /// <summary>
    /// View model for users sorting
    /// </summary>
    public class UsersSortViewModel
    {
        /// <summary>
        /// List of sorting variants
        /// </summary>
        public List<SelectListItem> SortBySelect { get; set; }

        /// <summary>
        /// Selected sort
        /// </summary>
        public int? SelectedSort { get; set; }

        /// <summary>
        /// Current sort
        /// </summary>
        public UsersSortState CurrentSort { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sortOrder"></param>
        public UsersSortViewModel(UsersSortState sortOrder)
        {
            CurrentSort = sortOrder;
        }
    }
}
