using Course_project.Models;
using Course_project.Models.ReviewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Helper
{
    internal class DatabaseInteractionHelper : GeneralHelper
    {
        private UserManager<User> userManager;

        private ApplicationDbContext db;

        internal DatabaseInteractionHelper(ApplicationDbContext context, UserManager<User> userManager)
        {
            db = context;
            this.userManager = userManager;
        }

        internal async Task<string> AddReviewToDb(string userName, Review review)
        {
            User user = await GetUserByNameAsync(userName);
            review.Id = Guid.NewGuid().ToString();
            review.AuthorId = user.Id;
            review.AuthorNickname = user.Nickname;
            review.AuthorUserName = user.UserName;
            review.PublicationDate = DateTime.Now;
            await db.Reviews.AddAsync(review);

            return review.Id;
        }

        internal async Task<User> GetUserByNameAsync(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        internal async Task<User> GetUserById(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        internal async Task<User> GetUserByEmailAsync(string userEmail)
        {
            return await userManager.FindByEmailAsync(userEmail);
        }

        internal async Task<Review> GetReviewAsync(string reviewId)
        {
            return await db.Reviews.FindAsync(reviewId);
        }

        internal async Task<Dictionary<int, string>> GetReviewGroupsAsync()
        {
            var reviewGroupsList = await db.ReviewGroups.ToListAsync();
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (var reviewGroup in reviewGroupsList)
            {
                result.Add(reviewGroup.Id, reviewGroup.Name);
            }

            return result;
        }

        internal async Task<List<ReviewImage>> GetReviewImagesAsync(string reviewId)
        {
            return await db.ReviewImages.Where(p => p.ReviewId == reviewId).ToListAsync();
        }

        internal async Task DeleteReviewImages(string reviewId)
        {
            List<ReviewImage> reviewImages = await db.ReviewImages.Where(p => p.ReviewId == reviewId).ToListAsync();
            db.ReviewImages.RemoveRange(reviewImages);
            await db.SaveChangesAsync();
        }

        internal async Task<List<Comment>> GetReviewCommentsAsync(string reviewId)
        {
            IQueryable<Comment> reviewComments = db.Comments;
            List<Comment> result = await reviewComments.Where(p => p.ReviewId == reviewId).ToListAsync();

            return result;
        }

        internal async Task UpdateReviewInDb(Review updatedReview)
        {
            var review = await db.Reviews.FindAsync(updatedReview.Id);
            review.Update(updatedReview);
            await db.SaveChangesAsync();
        }

        internal async Task DeleteReviewInDb(string reviewId)
        {
            var review = await db.Reviews.FindAsync(reviewId);
            db.Reviews.Remove(review);
            await db.SaveChangesAsync();
        }

        internal async Task<List<Rating>> GetReviewRatings(string reviewId)
        {
            return await db.Ratings.Where(p => p.ReviewId == reviewId).ToListAsync();    
        }

        internal async Task<Rating> GetReviewRatingByUserName(string reviewId, string userName)
        {
            return await db.Ratings.Where(p => (p.ReviewId == reviewId) && (p.UserName == userName)).FirstOrDefaultAsync();
        }

        internal async Task AddRatingToDb(string userName, string reviewId, int userVoice)
        {
            User user = await GetUserByNameAsync(userName);
            var rating = new Rating()
            {
                Id = Guid.NewGuid().ToString(),
                ReviewId = reviewId,
                UserVoice = userVoice,
                UserName = userName
            };
            Rating oldRating = await GetReviewRatingByUserName(reviewId, userName);
            if (oldRating == null)
            {
                await db.Ratings.AddAsync(rating);
                await db.SaveChangesAsync();
            }
            else
            {
                oldRating.UserVoice = userVoice;
                await db.SaveChangesAsync();
            }
        }

        internal async Task UpdateAverageRating(string reviewId)
        {
            List<Rating> reviewRatings = await GetReviewRatings(reviewId);
            int ratingSum = 0;
            foreach (var rating in reviewRatings)
            {
                ratingSum += rating.UserVoice;
            }
            Review review = await GetReviewAsync(reviewId);
            review.AverageRating = ratingSum / reviewRatings.Count;
            await db.SaveChangesAsync();
        }
    }
}
