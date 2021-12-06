using Course_project.Models;
using System;
using System.Collections.Generic;

namespace Course_project.ViewModels.HomeAuthorized
{
    /// <summary>
    /// View model for ReadReview page
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
        /// View model for rating
        /// </summary>
        public RatingViewModel RatingViewModel { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User nickname
        /// </summary>
        public string UserNickname { get; set; }

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
