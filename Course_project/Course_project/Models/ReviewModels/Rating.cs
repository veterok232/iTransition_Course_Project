using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models.ReviewModels
{
    public class Rating
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string ReviewId { get; set; }

        public int UserVoice { get; set; }
    }
}
