using Course_project.Models;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using System;
using System.Collections.Generic;

namespace Course_project.ViewModels.Account
{
    /// <summary>
    /// View model for PersonalAccount page
    /// </summary>
    public class PersonalAccountViewModel
    {
        /// <summary>
        /// User id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User nickname
        /// </summary>
        public string UserNickname { get; set; }

        /// <summary>
        /// List of reviews
        /// </summary>
        public List<Review> Reviews { get; set; }

        /// <summary>
        /// Dictinary of review groups
        /// </summary>
        public Dictionary<int, string> ReviewGroups { get; set; }

        /// <summary>
        /// View model for reviews filtering
        /// </summary>
        public FilterViewModel FilterViewModel { get; set; }

        /// <summary>
        /// View model for reviews sorting
        /// </summary>
        public SortViewModel SortViewModel { get; set; }

        /// <summary>
        /// View model for reviews paging
        /// </summary>
        public PageViewModel PageViewModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PersonalAccountViewModel()
        {
            Reviews = new List<Review>();
            ReviewGroups = new Dictionary<int, string>();
        }
    }
}
