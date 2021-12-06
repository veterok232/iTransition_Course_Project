using Course_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.HomeGuest
{
    public class ReadReviewViewModel
    {
        public Review Review { get; set; }

        public Dictionary<int, string> ReviewGroups { get; set; }

        public List<ReviewImage> ReviewImages { get; set; }

        public List<Comment> ReviewComments { get; set; }

        public ReadReviewViewModel()
        {
            Review = new Review();
            ReviewGroups = new Dictionary<int, string>();
            ReviewImages = new List<ReviewImage>();
            ReviewComments = new List<Comment>();
        }
    }
}
