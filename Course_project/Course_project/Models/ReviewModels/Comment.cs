using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models
{
    /// <summary>
    /// Comment model class
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Comment Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Comment review id
        /// </summary>
        public string ReviewId { get; set; }

        /// <summary>
        /// Comment author id
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Comment author nickname
        /// </summary>
        public string AuthorNickName { get; set; }

        /// <summary>
        /// Comment author user name
        /// </summary>
        public string AuthorUserName { get; set; }

        /// <summary>
        /// Comment publication date
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Comment text
        /// </summary>
        public string Text { get; set; }
    }
}
