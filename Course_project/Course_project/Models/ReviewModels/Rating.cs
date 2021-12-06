using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models.ReviewModels
{
    /// <summary>
    /// Rating model class
    /// </summary>
    public class Rating
    {
        /// <summary>
        /// Rating id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User name of voter
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Rating review id
        /// </summary>
        public string ReviewId { get; set; }

        /// <summary>
        /// User voice
        /// </summary>
        public int UserVoice { get; set; }
    }
}
