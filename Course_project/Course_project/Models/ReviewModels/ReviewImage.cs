using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_project.Models
{
    /// <summary>
    /// Review image model class
    /// </summary>
    public class ReviewImage
    {
        /// <summary>
        /// Review image id
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Review id
        /// </summary>
        [Required]
        public string ReviewId { get; set; }

        /// <summary>
        /// Image URL
        /// </summary>
        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "ImageURL")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Image file
        /// </summary>
        [Display(Name = "Image file")]
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        /// <summary>
        /// Image storage name
        /// </summary>
        [Required]
        public string ImageStorageName { get; set; }

        /// <summary>
        /// Image file name
        /// </summary>
        [Required]
        [Display(Name = "Image file name")]
        public string ImageFileName { get; set; }
    }
}
