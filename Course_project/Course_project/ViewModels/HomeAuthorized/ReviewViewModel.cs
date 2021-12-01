using Course_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.HomeAuthorized
{
    public class ReviewViewModel
    {
        public Review Review { get; set; }

        public List<SelectListItem> ReviewGroups { get; set; }
        
        public List<SelectListItem> ReviewMarks { get; set; }

        public IFormFileCollection ReviewImagesFiles { get; set; }

        public List<ReviewImage> ReviewImages { get; set; }

        public List<ReviewTag> ReviewTags { get; set; }

        public List<Comment> ReviewComments { get; set; }
    }
}
