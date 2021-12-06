using Course_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Course_project.ViewModels.HomeAuthorized
{
    /// <summary>
    /// View model for Review
    /// </summary>
    public class ReviewViewModel
    {
        /// <summary>
        /// Review
        /// </summary>
        public Review Review { get; set; }

        /// <summary>
        /// List of review groups
        /// </summary>
        public List<SelectListItem> ReviewGroups { get; set; }

        /// <summary>
        /// List of review marks
        /// </summary>
        public List<SelectListItem> ReviewMarks { get; set; }

        /// <summary>
        /// Collection of review images
        /// </summary>
        public IFormFileCollection ReviewImagesFiles { get; set; }

        /// <summary>
        /// List of review images
        /// </summary>
        public List<ReviewImage> ReviewImages { get; set; }

        /// <summary>
        /// List of review tags
        /// </summary>
        public List<ReviewTag> ReviewTags { get; set; }

        /// <summary>
        /// List of review comments
        /// </summary>
        public List<Comment> ReviewComments { get; set; }

        /// <summary>
        /// User nickname
        /// </summary>
        public string UserNickname { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewViewModel()
        {
            Review = new Review();
            ReviewGroups = new List<SelectListItem>();
            ReviewMarks = new List<SelectListItem>();
            ReviewImages = new List<ReviewImage>();
            ReviewTags = new List<ReviewTag>();
            ReviewComments = new List<Comment>();
        }
    }
}
