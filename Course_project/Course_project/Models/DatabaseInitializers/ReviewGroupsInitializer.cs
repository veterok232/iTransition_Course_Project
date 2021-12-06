using System;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models.DatabaseInitializers
{
    /// <summary>
    /// Initializer class for review groups
    /// </summary>
    public class ReviewGroupsInitializer
    {
        /// <summary>
        /// Initialize review groups in database
        /// </summary>
        /// <param name="context">Database context</param>
        /// <returns>Task</returns>
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            if (!context.ReviewGroups.Any())
            {
                await context.ReviewGroups.AddRangeAsync(
                    new ReviewGroup() { Name = "Films" },
                    new ReviewGroup() { Name = "Books" },
                    new ReviewGroup() { Name = "Games" }
                    );

                await context.SaveChangesAsync();
            }
        }
    }
}
