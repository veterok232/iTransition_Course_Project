using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models
{
    public class ReviewImage
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageURL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Image file")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [Required]
        public string ImageStorageName { get; set; }

        [Required]
        [Display(Name = "Image file name")]
        public string ImageFileName { get; set; }
    }
}
