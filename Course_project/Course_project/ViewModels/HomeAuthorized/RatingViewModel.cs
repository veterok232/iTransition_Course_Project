using System;

namespace Course_project.ViewModels.HomeAuthorized
{
    /// <summary>
    /// View model for Rating
    /// </summary>
    public class RatingViewModel
    {
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Review id
        /// </summary>
        public string ReviewId { get; set; }

        /// <summary>
        /// User voice
        /// </summary>
        public int UserVoice { get; set; }
    }
}
