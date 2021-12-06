using Course_project.Models;
using Course_project.ViewModels.UsersFilterSortPagination;
using System;
using System.Collections.Generic;

namespace Course_project.ViewModels.Account
{
    /// <summary>
    /// View model for AdminAccount
    /// </summary>
    public class AdminAccountViewModel
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
        /// List of users
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// View model for users filtering
        /// </summary>
        public UsersFilterViewModel FilterViewModel { get; set; }

        /// <summary>
        /// View model for users sorting
        /// </summary>
        public UsersSortViewModel SortViewModel { get; set; }

        /// <summary>
        /// View model for users paging
        /// </summary>
        public UsersPageViewModel PageViewModel { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AdminAccountViewModel()
        {
            Users = new List<User>();
        }
    }
}
