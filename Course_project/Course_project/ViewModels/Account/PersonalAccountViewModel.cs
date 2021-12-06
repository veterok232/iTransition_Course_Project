using Course_project.Models;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.Account
{
    public class PersonalAccountViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserNickname { get; set; }

        public List<Review> Reviews { get; set; }

        public Dictionary<int, string> ReviewGroups { get; set; }

        public FilterViewModel FilterViewModel { get; set; }

        public SortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public PersonalAccountViewModel()
        {
            Reviews = new List<Review>();
            ReviewGroups = new Dictionary<int, string>();
        }
    }
}
