using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.ReviewsFilterSortPagination
{
    /// <summary>
    /// View model for sorting reviews
    /// </summary>
    public class SortViewModel
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
        public SortState CurrentSort { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sortOrder">Sort order</param>
        public SortViewModel(SortState sortOrder)
        {
            CurrentSort = sortOrder;
        }
    }
}
