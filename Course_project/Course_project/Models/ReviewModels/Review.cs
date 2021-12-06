using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Course_project.Models
{
    /// <summary>
    /// Review model class
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Review id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Review author id
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Review author user name
        /// </summary>
        public string AuthorUserName { get; set; }

        /// <summary>
        /// Review author nickname
        /// </summary>
        public string AuthorNickname { get; set; }

        /// <summary>
        /// Review object title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Review title
        /// </summary>
        public string ReviewTitle { get; set; }

        /// <summary>
        /// Review group id
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Review text
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        /// <summary>
        /// Review author mark
        /// </summary>
        public int AuthorMark { get; set; }

        /// <summary>
        /// Review likes count
        /// </summary>
        public int LikesCount { get; set; }

        /// <summary>
        /// Review publication or modification date
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Review comments count
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Review average rating
        /// </summary>
        public int AverageRating { get; set; }

        /// <summary>
        /// Review preview image URL
        /// </summary>
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; } = "https://storage.googleapis.com/itransitioncourseproject-veterok232-bucket/no-image.jpg";
        
        /// <summary>
        /// Update review from another review object
        /// </summary>
        /// <param name="review"></param>
        public void Update(Review review)
        {
            Title = review.Title;
            ReviewTitle = review.ReviewTitle;
            GroupId = review.GroupId;
            Text = review.Text;
            AuthorMark = review.AuthorMark;
            ImageUrl = review.ImageUrl;
            PublicationDate = DateTime.Now;
        }

    }
}
