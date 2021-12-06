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
    /// <summary>
    /// Helper for database interactions
    /// </summary>
    internal class DatabaseInteractionHelper : GeneralHelper
    {
        /// <summary>
        /// User manager
        /// </summary>
        private UserManager<User> userManager;

        /// <summary>
        /// Databse context
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// Constructor for DatabaseInteractionHelper class
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="userManager">User manager</param>
        internal DatabaseInteractionHelper(ApplicationDbContext context, UserManager<User> userManager)
        {
            db = context;
            this.userManager = userManager;
        }

        /// <summary>
        /// Add review to databse
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="review">Review</param>
        /// <returns>Task<string> - review id</returns>
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

        /// <summary>
        /// Get user by name from database
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>Task<User></returns>
        internal async Task<User> GetUserByNameAsync(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        /// <summary>
        /// Get user by id from database
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Task<User></returns>
        internal async Task<User> GetUserById(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        /// <summary>
        /// Get user by email from database
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <returns>Task<User></returns>
        internal async Task<User> GetUserByEmailAsync(string userEmail)
        {
            return await userManager.FindByEmailAsync(userEmail);
        }

        /// <summary>
        /// Get review by reviewId from database
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<Review></returns>
        internal async Task<Review> GetReviewAsync(string reviewId)
        {
            return await db.Reviews.FindAsync(reviewId);
        }

        /// <summary>
        /// Get ReviewGroups from database
        /// </summary>
        /// <returns>Task<Dictionary<int, string>></returns>
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

        /// <summary>
        /// Get review images from database
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<List<ReviewImage>></returns>
        internal async Task<List<ReviewImage>> GetReviewImagesAsync(string reviewId)
        {
            return await db.ReviewImages.Where(p => p.ReviewId == reviewId).ToListAsync();
        }

        /// <summary>
        /// Delete review images from database
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task</returns>
        internal async Task DeleteReviewImages(string reviewId)
        {
            List<ReviewImage> reviewImages = await db.ReviewImages.Where(p => p.ReviewId == reviewId).ToListAsync();
            db.ReviewImages.RemoveRange(reviewImages);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Get review comments from database
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<List<Comment>></returns>
        internal async Task<List<Comment>> GetReviewCommentsAsync(string reviewId)
        {
            IQueryable<Comment> reviewComments = db.Comments;
            List<Comment> result = await reviewComments.Where(p => p.ReviewId == reviewId).ToListAsync();

            return result;
        }

        /// <summary>
        /// Update review in database
        /// </summary>
        /// <param name="updatedReview">Updated review</param>
        /// <returns>Task</returns>
        internal async Task UpdateReviewInDb(Review updatedReview)
        {
            var review = await db.Reviews.FindAsync(updatedReview.Id);
            review.Update(updatedReview);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Delete review from database
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task</returns>
        internal async Task DeleteReviewInDb(string reviewId)
        {
            var review = await db.Reviews.FindAsync(reviewId);
            db.Reviews.Remove(review);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Get review ratings from database
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<List<Rating>></returns>
        internal async Task<List<Rating>> GetReviewRatings(string reviewId)
        {
            return await db.Ratings.Where(p => p.ReviewId == reviewId).ToListAsync();    
        }

        /// <summary>
        /// Get review rating by user name
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <param name="userName">User name</param>
        /// <returns>Task<Rating></returns>
        internal async Task<Rating> GetReviewRatingByUserName(string reviewId, string userName)
        {
            return await db.Ratings.Where(p => (p.ReviewId == reviewId) && (p.UserName == userName)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Add rating to database
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="reviewId">Review Id</param>
        /// <param name="userVoice">User voice</param>
        /// <returns>Rask</returns>
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

        /// <summary>
        /// Update average rating for review in database
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task</returns>
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
