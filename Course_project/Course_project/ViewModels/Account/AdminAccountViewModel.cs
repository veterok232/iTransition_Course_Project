using Course_project.Models;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using Course_project.ViewModels.UsersFilterSortPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.Account
{
    public class AdminAccountViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserNickname { get; set; }

        public List<User> Users { get; set; }

        public UsersFilterViewModel FilterViewModel { get; set; }

        public UsersSortViewModel SortViewModel { get; set; }

        public UsersPageViewModel PageViewModel { get; set; }

        public AdminAccountViewModel()
        {
            Users = new List<User>();
        }
    }
}
