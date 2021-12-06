using Course_project.Models;
using Course_project.ViewModels.HomeAuthorized;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using Course_project.Helper;
using Course_project.CloudStorage;
using Microsoft.AspNetCore.Http;
using System;
using Course_project.Services;

namespace Course_project.Controllers
{
    /// <summary>
    /// Controller for authorized user
    /// </summary>
    [Authorize]
    public class HomeAuthorizedController : Controller
    {
        /// <summary>
        /// Database context
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// User manager
        /// </summary>
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Cloud storage
        /// </summary>
        private readonly ICloudStorage cloudStorage;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<HomeAuthorizedController> _logger;

        /// <summary>
        /// Helper for HomeAuthorizedController
        /// </summary>
        private HomeAuthorizedHelper helper;

        /// <summary>
        /// Helper for database interactions
        /// </summary>
        private DatabaseInteractionHelper databaseHelper;

        /// <summary>
        /// Constructor for HomeAuthorizedController class
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="userManager">User manager</param>
        /// <param name="context">Database context</param>
        /// <param name="cloudStorage">Cloud storage</param>
        public HomeAuthorizedController(
            ILogger<HomeAuthorizedController> logger, 
            UserManager<User> userManager, 
            ApplicationDbContext context,
            ICloudStorage cloudStorage)
        {
            this.userManager = userManager;
            this.cloudStorage = cloudStorage;
            _logger = logger;
            db = context;
            helper = new HomeAuthorizedHelper(context, cloudStorage, userManager);
            databaseHelper = new DatabaseInteractionHelper(context, userManager);
        }

        /// <summary>
        /// Index GET action
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await helper.GetIndexViewModel(User.Identity.Name));
        }

        /// <summary>
        /// Crete review GET action
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> CreateReview()
        {
            return View(await helper.GetReviewViewModel(User.Identity));
        }

        /// <summary>
        /// Create review POST action
        /// </summary>
        /// <param name="viewModel">ReviewViewModel</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        public async Task<IActionResult> CreateReview(
            ReviewViewModel viewModel)
        {
            string reviewId = await databaseHelper.AddReviewToDb(User.Identity.Name, viewModel.Review);
            string reviewImageUrl = await helper.AddReviewImagesToDb(viewModel.ReviewImagesFiles, reviewId);
            if (reviewImageUrl != null)
            {
                viewModel.Review.ImageUrl = reviewImageUrl;
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "HomeAuthorized");
        }

        /// <summary>
        /// Reviews GET action
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="author">Author</param>
        /// <param name="groupId">Group Id</param>
        /// <param name="sortOrder">Sort order</param>
        /// <param name="page">Page</param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> Reviews(string title, string author, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            return View(await helper.GetReviewsViewModel(User.Identity.Name, title, author, groupId, sortOrder, page));
        }

        /// <summary>
        /// ReadReview GET action
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> ReadReview(string reviewId)
        {
            return View(await helper.GetReadReviewViewModel(User.Identity.Name, reviewId)); 
        }

        /// <summary>
        /// EditReview GET action
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> EditReview(string reviewId)
        {
            return View(await helper.GetEditReviewViewModel(User.Identity.Name, reviewId));
        }

        /// <summary>
        /// EditReview POST action
        /// </summary>
        /// <param name="viewModel">ReviewViewModel</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        public async Task<IActionResult> EditReview(ReviewViewModel viewModel)
        {
            string reviewImageUrl = await helper.UpdateReviewImagesInDb(viewModel.ReviewImagesFiles, viewModel.Review.Id);
            if (reviewImageUrl != null)
            {
                viewModel.Review.ImageUrl = reviewImageUrl;
            }
            await databaseHelper.UpdateReviewInDb(viewModel.Review);
            await db.SaveChangesAsync();

            return RedirectToAction("Index", "HomeAuthorized");
        }

        /// <summary>
        /// DeleteReview GET action
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteReview(string reviewId)
        {
            return View(await helper.GetDeleteReviewViewModel(reviewId, User.Identity.Name));
        }

        /// <summary>
        /// DeleteReview POST action
        /// </summary>
        /// <param name="viewModel">DeleteReviewViewModel</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteReview(DeleteReviewViewModel viewModel)
        {
            if (!String.IsNullOrEmpty(viewModel.ReviewId))
            {
                await databaseHelper.DeleteReviewInDb(viewModel.ReviewId);
                await databaseHelper.DeleteReviewImages(viewModel.ReviewId);
            }
            
            return RedirectToAction("Index", "HomeAuthorized");
        }

        /// <summary>
        /// Vote for review POST action
        /// </summary>
        /// <param name="viewModel">ReadReviewViewModel</param>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        public async Task<IActionResult> VoteForReview(ReadReviewViewModel viewModel)
        {
            await databaseHelper.AddRatingToDb(
                viewModel.RatingViewModel.UserName,
                viewModel.RatingViewModel.ReviewId,
                viewModel.RatingViewModel.UserVoice);
            await databaseHelper.UpdateAverageRating(
                viewModel.RatingViewModel.ReviewId);
            return LocalRedirect($"~/HomeAuthorized/ReadReview?reviewId={viewModel.RatingViewModel.ReviewId}");
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns>IActionResult</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
