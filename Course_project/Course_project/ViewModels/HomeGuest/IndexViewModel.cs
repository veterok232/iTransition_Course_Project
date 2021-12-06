using Course_project.Models;
using System;
using System.Collections.Generic;

namespace Course_project.ViewModels.HomeGuest
{
    /// <summary>
    /// View model for Index page
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// List of most rating reviews
        /// </summary>
        public List<Review> ReviewsMostRating { get; set; }

        /// <summary>
        /// List of last reviews
        /// </summary>
        public List<Review> ReviewsLast { get; set; }

        /// <summary>
        /// Dictionary of review groups
        /// </summary>
        public Dictionary<int, string> ReviewGroups { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public IndexViewModel()
        {
            ReviewsMostRating = new List<Review>();
            ReviewsLast = new List<Review>();
            ReviewGroups = new Dictionary<int, string>();
        }
    }
}
