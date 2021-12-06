using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.HomeAuthorized
{
    public class DeleteReviewViewModel
    {
        public string UserNickname { get; set; }

        public string ReviewId { get; set; }

        public string ReturnUrl { get; set; }
    }
}
