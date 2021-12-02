using Course_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.HomeAuthorized
{
    public class ReviewsViewModel
    {
        public List<Review> Reviews { get; set; }

        public Dictionary<int, string> ReviewGroups { get; set; }

        public string UserNickname { get; set; }

        public string UserName { get; set; }

        public FilterViewModel FilterViewModel { get; set; } 

        public SortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }

        public ReviewsViewModel()
        {
            Reviews = new List<Review>();
            ReviewGroups = new Dictionary<int, string>();
        }
    }
}
