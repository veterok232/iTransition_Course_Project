using System;

namespace Course_project.ViewModels.HomeAuthorized
{
    /// <summary>
    /// View model for DeleteReview page
    /// </summary>
    public class DeleteReviewViewModel
    {
        /// <summary>
        /// User nickname
        /// </summary>
        public string UserNickname { get; set; }

        /// <summary>
        /// Reveiw id
        /// </summary>
        public string ReviewId { get; set; }

        /// <summary>
        /// Return URL
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
