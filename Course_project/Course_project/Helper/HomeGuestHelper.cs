using Course_project.Models;
using Course_project.ViewModels.HomeGuest;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Helper
{
    internal class HomeGuestHelper : GeneralHelper
    {
        private ApplicationDbContext db;

        private DatabaseInteractionHelper databaseHelper;

        internal HomeGuestHelper(ApplicationDbContext context)
        {
            db = context;
            databaseHelper = new DatabaseInteractionHelper(context, null);
        }

        internal async Task<IndexViewModel> GetIndexViewModel()
        {
            var viewModel = new IndexViewModel();
            viewModel.ReviewGroups = await databaseHelper.GetReviewGroupsAsync();
            IQueryable<Review> reviews = db.Reviews;
            SortReviews(ref reviews, SortState.RatingDesc);
            viewModel.ReviewsMostRating = await GetPageReviews(reviews, 1, INDEX_PAGE_SIZE);
            reviews = db.Reviews;
            SortReviews(ref reviews, SortState.PublicationDateDesc);
            viewModel.ReviewsLast = await GetPageReviews(reviews, 1, INDEX_PAGE_SIZE);

            return viewModel;
        }

        internal async Task<ReviewsViewModel> GetReviewsViewModel(string title, string author, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            var viewModel = new ReviewsViewModel();
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

        internal async Task<ReadReviewViewModel> GetReadReviewViewModel(string reviewId)
        {
            return new ReadReviewViewModel()
            {
                Review = await databaseHelper.GetReviewAsync(reviewId),
                ReviewGroups = await databaseHelper.GetReviewGroupsAsync(),
                ReviewImages = await databaseHelper.GetReviewImagesAsync(reviewId),
                ReviewComments = await databaseHelper.GetReviewCommentsAsync(reviewId),
            };
        }
    }
}
