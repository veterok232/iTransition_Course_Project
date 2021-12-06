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
    [Authorize]
    public class HomeAuthorizedController : Controller
    {
        private ApplicationDbContext db;

        private readonly UserManager<User> userManager;

        private readonly ICloudStorage cloudStorage;

        private readonly ILogger<HomeAuthorizedController> _logger;

        private HomeAuthorizedHelper helper;

        private DatabaseInteractionHelper databaseHelper;

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

        public async Task<IActionResult> Index()
        {
            return View(await helper.GetIndexViewModel(User.Identity.Name));
        }

        [HttpGet]
        public async Task<IActionResult> CreateReview()
        {
            return View(await helper.GetReviewViewModel(User.Identity));
        }

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

        [HttpGet]
        public async Task<IActionResult> Reviews(string title, string author, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            return View(await helper.GetReviewsViewModel(User.Identity.Name, title, author, groupId, sortOrder, page));
        }

        [HttpGet]
        public async Task<IActionResult> ReadReview(string reviewId)
        {
            return View(await helper.GetReadReviewViewModel(User.Identity.Name, reviewId)); 
        }

        [HttpGet]
        public async Task<IActionResult> EditReview(string reviewId)
        {
            return View(await helper.GetEditReviewViewModel(User.Identity.Name, reviewId));
        }

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

        [HttpGet]
        public async Task<IActionResult> DeleteReview(string reviewId)
        {
            return View(await helper.GetDeleteReviewViewModel(reviewId, User.Identity.Name));
        }

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
