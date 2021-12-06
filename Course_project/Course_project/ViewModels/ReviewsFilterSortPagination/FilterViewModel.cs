using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Course_project.ViewModels.ReviewsFilterSortPagination
{
    /// <summary>
    /// View model for filtering reviews
    /// </summary>
    public class FilterViewModel
    {
        /// <summary>
        /// Review groups select
        /// </summary>
        public IEnumerable<SelectListItem> ReviewGroupsSelect { get; set; }

        /// <summary>
        /// Selected group
        /// </summary>
        public int? SelectedGroup { get; set; }

        /// <summary>
        /// Title filter
        /// </summary>
        public string TitleFilter { get; set; }

        /// <summary>
        /// Author filter
        /// </summary>
        public string AuthorFilter { get; set; }
    }
}
