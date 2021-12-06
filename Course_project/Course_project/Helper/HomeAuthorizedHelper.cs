using Course_project.Models;
using Course_project.ViewModels.HomeAuthorized;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Course_project.CloudStorage;
using System.IO;
using Microsoft.AspNetCore.Http;
using Course_project.ViewModels.ReviewsFilterSortPagination;

namespace Course_project.Helper
{
    /// <summary>
    /// Helper for HomeAuthorizedController
    /// </summary>
    internal class HomeAuthorizedHelper : GeneralHelper
    {
        /// <summary>
        /// Cloud storage
        /// </summary>
        private ICloudStorage cloudStorage;

        /// <summary>
        /// User manager
        /// </summary>
        private UserManager<User> userManager;

        /// <summary>
        /// Database context
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// Helper for database interactions
        /// </summary>
        private DatabaseInteractionHelper databaseHelper;

        /// <summary>
        /// Constructor for HomeAuthorizedHelper class
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="cloudStorage">Cloud storage</param>
        /// <param name="userManager">User manager</param>
        internal HomeAuthorizedHelper(
            ApplicationDbContext context, 
            ICloudStorage cloudStorage, 
            UserManager<User> userManager)
        {
            this.cloudStorage = cloudStorage;
            this.userManager = userManager;
            db = context;
            databaseHelper = new DatabaseInteractionHelper(context, userManager);
        }

        /// <summary>
        /// Get IndexViewModel
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>Task<IndexViewModel></returns>
        internal async Task<IndexViewModel> GetIndexViewModel(string userName)
        {
            var viewModel = new IndexViewModel();
            viewModel.UserName = userName;
            viewModel.UserNickname = (await databaseHelper.GetUserByNameAsync(userName)).Nickname;
            viewModel.ReviewGroups = await databaseHelper.GetReviewGroupsAsync();
            IQueryable<Review> reviews = db.Reviews;
            SortReviews(ref reviews, SortState.RatingDesc);
            viewModel.ReviewsMostRating = await GetPageReviews(reviews, 1, INDEX_PAGE_SIZE);
            reviews = db.Reviews;
            SortReviews(ref reviews, SortState.PublicationDateDesc);
            viewModel.ReviewsLast = await GetPageReviews(reviews, 1, INDEX_PAGE_SIZE);

            return viewModel;
        }

        /// <summary>
        /// Get ReviewViewModel
        /// </summary>
        /// <param name="userIdentity">User identity</param>
        /// <returns>Task<ReviewViewModel></returns>
        internal async Task<ReviewViewModel> GetReviewViewModel(IIdentity userIdentity)
        {
            var user = await userManager.FindByNameAsync(userIdentity.Name);
            var reviewGroups = db.ReviewGroups.ToList();
            
            return new ReviewViewModel()
            {
                Review = new Review()
                {
                    AuthorId = user.Id,
                    AuthorUserName = user.UserName,
                    AuthorNickname = user.Nickname,
                },
                ReviewGroups = CreateReviewGroupsList(reviewGroups),
                ReviewMarks = CreateReviewMarksList(),
                ReviewImages = null,
                ReviewComments = new List<Comment>(),
                ReviewTags = new List<ReviewTag>(),
                UserNickname = user.Nickname
            };
        }

        /// <summary>
        /// Upload image to Google Cloud
        /// </summary>
        /// <param name="reviewImage">Review image</param>
        /// <returns>Task</returns>
        internal async Task UploadImage(ReviewImage reviewImage)
        {
            string fileNameForStorage = CreateFileName(reviewImage.ImageFile.FileName);
            reviewImage.ImageUrl = await cloudStorage.UploadFileAsync(reviewImage.ImageFile, fileNameForStorage);
            reviewImage.ImageStorageName = fileNameForStorage;
            reviewImage.ImageFileName = reviewImage.ImageFile.FileName;
        }

        /// <summary>
        /// Add review images to database
        /// </summary>
        /// <param name="uploads">Images to upload</param>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<string></returns>
        internal async Task<string> AddReviewImagesToDb(IFormFileCollection uploads, string reviewId)
        {
            var reviewImages = new List<ReviewImage>();
            if (uploads != null)
            {
                foreach (var uploadedFile in uploads)
                {
                    var reviewImage = new ReviewImage() { Id = Guid.NewGuid().ToString(), ImageFile = uploadedFile, ReviewId = reviewId };
                    await UploadImage(reviewImage);
                    reviewImages.Add(reviewImage);
                }
            }
            db.ReviewImages.AddRange(reviewImages);

            return reviewImages.Count > 0 ? reviewImages[0].ImageUrl : null;
        }

        /// <summary>
        /// Update review images in database
        /// </summary>
        /// <param name="uploads">Uploads</param>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<string></returns>
        internal async Task<string> UpdateReviewImagesInDb(IFormFileCollection uploads, string reviewId)
        {
            await databaseHelper.DeleteReviewImages(reviewId);
            return await AddReviewImagesToDb(uploads, reviewId);
        }

        /// <summary>
        /// Get ReadReviewViewModel
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<ReadReviewViewModel></returns>
        internal async Task<ReadReviewViewModel> GetReadReviewViewModel(string userName, string reviewId)
        {
            return new ReadReviewViewModel()
            {
                Review = await databaseHelper.GetReviewAsync(reviewId),
                ReviewGroups = await databaseHelper.GetReviewGroupsAsync(),
                ReviewImages = await databaseHelper.GetReviewImagesAsync(reviewId),
                ReviewComments = await databaseHelper.GetReviewCommentsAsync(reviewId),
                UserNickname = (await databaseHelper.GetUserByNameAsync(userName)).Nickname,
                UserName = userName
            };
        }

        /// <summary>
        /// Get ReviewsViewModel
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="title">Title</param>
        /// <param name="author">Author</param>
        /// <param name="groupId">Group Id</param>
        /// <param name="sortOrder">Sort order</param>
        /// <param name="page">Page</param>
        /// <returns>Task<ReviewsViewModel></returns>
        internal async Task<ReviewsViewModel> GetReviewsViewModel(
            string userName, string title, string author, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            var viewModel = new ReviewsViewModel();
            viewModel.UserName = userName;
            viewModel.UserNickname = (await databaseHelper.GetUserByNameAsync(userName)).Nickname;
            viewModel.ReviewGroups = await databaseHelper.GetReviewGroupsAsync();
            IQueryable<Review> reviews = db.Reviews;
            FilterReviews(ref reviews, title, author, groupId);
            SortReviews(ref reviews, sortOrder);
            viewModel.Reviews = await GetPageReviews(reviews, page, REVIEWS_PAGE_SIZE);        
            viewModel.FilterViewModel = GetFilterViewModel(viewModel.ReviewGroups.Values.ToList(), groupId, title, author);
            viewModel.SortViewModel = GetSortViewModel(sortOrder);
            viewModel.PageViewModel = GetPageViewModel(await GetReviewsCount(reviews), page, REVIEWS_PAGE_SIZE);
            
            return viewModel;
        }

        /// <summary>
        /// Get ReviewViewModel for edit review
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<ReviewViewModel></returns>
        internal async Task<ReviewViewModel> GetEditReviewViewModel(string userName, string reviewId)
        {
            var viewModel = new ReviewViewModel();
            viewModel.Review = await databaseHelper.GetReviewAsync(reviewId);
            viewModel.ReviewGroups = CreateReviewGroupsList(db.ReviewGroups.ToList());
            viewModel.ReviewGroups[viewModel.Review.GroupId].Selected = true;
            viewModel.ReviewMarks = CreateReviewMarksList();
            viewModel.ReviewMarks[viewModel.Review.AuthorMark - 1].Selected = true;
            viewModel.ReviewImages = await databaseHelper.GetReviewImagesAsync(reviewId);
            viewModel.ReviewComments = await databaseHelper.GetReviewCommentsAsync(reviewId);
            viewModel.ReviewTags = new List<ReviewTag>();
            viewModel.UserNickname = (await databaseHelper.GetUserByNameAsync(userName)).Nickname;

            return viewModel;
        }

        /// <summary>
        /// Get DeleteReviewViewModel
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <param name="userName">User name</param>
        /// <returns>Task<DeleteReviewViewModel></returns>
        internal async Task<DeleteReviewViewModel> GetDeleteReviewViewModel(string reviewId, string userName)
        {
            return new DeleteReviewViewModel()
            {
                UserNickname = (await databaseHelper.GetUserByNameAsync(userName)).Nickname,
                ReviewId = reviewId
            };
        }

        /// <summary>
        /// Create unique file name for storage in Google Cloud
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string CreateFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var fileNameForStorage = $"{fileNameWithoutExt}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";

            return fileNameForStorage;
        }

        /// <summary>
        /// Create ReviewGroups select list
        /// </summary>
        /// <param name="reviewGroups">Review groups</param>
        /// <returns>List<SelectListItem></returns>
        private List<SelectListItem> CreateReviewGroupsList(List<ReviewGroup> reviewGroups)
        {
            var result = new List<SelectListItem>();
            foreach (var reviewGroup in reviewGroups)
            {
                result.Add(new SelectListItem() { Text = reviewGroup.Name, Value = reviewGroup.Id.ToString() });
            }

            return result;
        }

        /// <summary>
        /// Create review marks select list
        /// </summary>
        /// <returns>List<SelectListItem></returns>
        private List<SelectListItem> CreateReviewMarksList()
        {
            var result = new List<SelectListItem>();
            for (int i = 1; i <= 10; i++)
            {
                result.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return result;
        }
    }
}
