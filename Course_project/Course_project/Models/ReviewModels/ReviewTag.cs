using System;

namespace Course_project.Models
{
    /// <summary>
    /// Review tag model class
    /// </summary>
    public class ReviewTag
    {
        /// <summary>
        /// Review tag id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Review id
        /// </summary>
        public int ReviewId { get; set; }

        /// <summary>
        /// Tag text
        /// </summary>
        public string Tag { get; set; }
    }
}
