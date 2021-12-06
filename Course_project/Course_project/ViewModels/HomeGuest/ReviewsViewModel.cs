using Course_project.Models;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using System;
using System.Collections.Generic;

namespace Course_project.ViewModels.HomeGuest
{
    /// <summary>
    /// View model for Reviews page
    /// </summary>
    public class ReviewsViewModel
    {
        /// <summary>
        /// Reviews
        /// </summary>
        public List<Review> Reviews { get; set; }

        /// <summary>
        /// Review groups
        /// </summary>
        public Dictionary<int, string> ReviewGroups { get; set; }

        /// <summary>
        /// View model for filtering reveiws
        /// </summary>
        public FilterViewModel FilterViewModel { get; set; }

        /// <summary>
        /// View model for sorting reveiws
        /// </summary>
        public SortViewModel SortViewModel { get; set; }

        /// <summary>
        /// View model for paging reveiws
        /// </summary>
        public PageViewModel PageViewModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewsViewModel()
        {
            Reviews = new List<Review>();
            ReviewGroups = new Dictionary<int, string>();
        }
    }
}
