using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models
{
    public class User : IdentityUser
    {
        public static object Identity { get; internal set; }
        public string Nickname { get; set; }
    }
}
