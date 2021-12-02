using System;
using System.Collections.Generic;

namespace Course_project.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorNickname { get; set; }

        public string Title { get; set; }

        public int GroupId { get; set; }

        public string Text { get; set; }

        public int AuthorMark { get; set; }

        public int LikesCount { get; set; }

        public DateTime PublicationDate { get; set; }

        public int CommentsCount { get; set; }

        public string ImageUrl { get; set; } = "https://storage.googleapis.com/itransitioncourseproject-veterok232-bucket/no-image.jpg";
    }
}
