using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models.DatabaseInitializers
{
    public class ReviewGroupsInitializer
    {
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
