using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string ReviewId { get; set; }

        public string AuthorId { get; set; }

        public string AuthorNickName { get; set; }

        public string AuthorUserName { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Text { get; set; }
    }
}
