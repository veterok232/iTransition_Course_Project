using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Course_project.Models
{
    public class Review
    {
        public string Id { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorNickname { get; set; }

        public string Title { get; set; }

        public string ReviewTitle { get; set; }

        public int GroupId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public int AuthorMark { get; set; }

        public int LikesCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        public int CommentsCount { get; set; }

        public int AverageRating { get; set; }

        [DataType(DataType.Url)]
        public string ImageUrl { get; set; } = "https://storage.googleapis.com/itransitioncourseproject-veterok232-bucket/no-image.jpg";
        
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
