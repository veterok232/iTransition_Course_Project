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
    internal class HomeAuthorizedHelper : GeneralHelper
    {
        private ICloudStorage cloudStorage;

        private UserManager<User> userManager;

        private ApplicationDbContext db;

        private DatabaseInteractionHelper databaseHelper;

        public HomeAuthorizedHelper(
            ApplicationDbContext context, 
            ICloudStorage cloudStorage, 
            UserManager<User> userManager)
        {
            this.cloudStorage = cloudStorage;
            this.userManager = userManager;
            db = context;
            databaseHelper = new DatabaseInteractionHelper(context, userManager);
        }

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

        internal async Task UploadImage(ReviewImage reviewImage)
        {
            string fileNameForStorage = CreateFileName(reviewImage.ImageFile.FileName);
            reviewImage.ImageUrl = await cloudStorage.UploadFileAsync(reviewImage.ImageFile, fileNameForStorage);
            reviewImage.ImageStorageName = fileNameForStorage;
            reviewImage.ImageFileName = reviewImage.ImageFile.FileName;
        }

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

        internal async Task<string> UpdateReviewImagesInDb(IFormFileCollection uploads, string reviewId)
        {
            await databaseHelper.DeleteReviewImages(reviewId);
            return await AddReviewImagesToDb(uploads, reviewId);
        }

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

        internal async Task<DeleteReviewViewModel> GetDeleteReviewViewModel(string reviewId, string userName)
        {
            return new DeleteReviewViewModel()
            {
                UserNickname = (await databaseHelper.GetUserByNameAsync(userName)).Nickname,
                ReviewId = reviewId
            };
        }

        private string CreateFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var fileNameForStorage = $"{fileNameWithoutExt}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";

            return fileNameForStorage;
        }

        private List<SelectListItem> CreateReviewGroupsList(List<ReviewGroup> reviewGroups)
        {
            var result = new List<SelectListItem>();
            foreach (var reviewGroup in reviewGroups)
            {
                result.Add(new SelectListItem() { Text = reviewGroup.Name, Value = reviewGroup.Id.ToString() });
            }

            return result;
        }

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
