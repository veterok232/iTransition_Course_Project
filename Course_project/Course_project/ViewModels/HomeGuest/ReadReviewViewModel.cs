using Course_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.HomeGuest
{
    /// <summary>
    /// View model for ReadReveiw page
    /// </summary>
    public class ReadReviewViewModel
    {
        /// <summary>
        /// Review
        /// </summary>
        public Review Review { get; set; }

        /// <summary>
        /// Dictionary of review groups
        /// </summary>
        public Dictionary<int, string> ReviewGroups { get; set; }

        /// <summary>
        /// List of review images
        /// </summary>
        public List<ReviewImage> ReviewImages { get; set; }

        /// <summary>
        /// List of review comments
        /// </summary>
        public List<Comment> ReviewComments { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReadReviewViewModel()
        {
            Review = new Review();
            ReviewGroups = new Dictionary<int, string>();
            ReviewImages = new List<ReviewImage>();
            ReviewComments = new List<Comment>();
        }
    }
}
