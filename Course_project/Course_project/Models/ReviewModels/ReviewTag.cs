using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models
{
    public class ReviewTag
    {
        public int Id { get; set; }

        public int ReviewId { get; set; }

        public string Tag { get; set; }
    }
}
