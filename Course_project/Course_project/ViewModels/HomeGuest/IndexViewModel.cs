using Course_project.Models;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.HomeGuest
{
    public class IndexViewModel
    {
        public List<Review> ReviewsMostRating { get; set; }

        public List<Review> ReviewsLast { get; set; }

        public Dictionary<int, string> ReviewGroups { get; set; }

        public IndexViewModel()
        {
            ReviewsMostRating = new List<Review>();
            ReviewsLast = new List<Review>();
            ReviewGroups = new Dictionary<int, string>();
        }
    }
}
