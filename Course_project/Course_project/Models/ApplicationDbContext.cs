using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Course_project.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<FileModel> Files { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ReviewImage> ReviewImages { get; set; }

        public DbSet<ReviewTag> ReviewTags { get; set; }

        public DbSet<ReviewGroup> ReviewGroups { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
