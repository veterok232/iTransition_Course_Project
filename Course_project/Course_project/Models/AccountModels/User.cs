using Microsoft.AspNetCore.Identity;
using System;

namespace Course_project.Models
{
    /// <summary>
    /// User model class
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// User identity
        /// </summary>
        public static object Identity { get; internal set; }

        /// <summary>
        /// User nickname
        /// </summary>
        public string Nickname { get; set; }
    }
}
