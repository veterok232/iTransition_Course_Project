using Course_project.Models.ReviewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Course_project.Models
{
    /// <summary>
    /// Database context class
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Files table
        /// </summary>
        public DbSet<FileModel> Files { get; set; }

        /// <summary>
        /// Reviews table
        /// </summary>
        public DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// Comments table
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// ReviewImages table
        /// </summary>
        public DbSet<ReviewImage> ReviewImages { get; set; }

        /// <summary>
        /// ReviewTags table
        /// </summary>
        public DbSet<ReviewTag> ReviewTags { get; set; }

        /// <summary>
        /// ReviewGroups table
        /// </summary>
        public DbSet<ReviewGroup> ReviewGroups { get; set; }

        /// <summary>
        /// Ratings table
        /// </summary>
        public DbSet<Rating> Ratings { get; set; }

        /// <summary>
        /// Constructor for ApplicationDbContext
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
