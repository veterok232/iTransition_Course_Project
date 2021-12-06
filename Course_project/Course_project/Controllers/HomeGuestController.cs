using Course_project.Helper;
using Course_project.Models;
using Course_project.ViewModels.HomeGuest;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Course_project.Controllers
{
    /// <summary>
    /// Controller for unauthorized users
    /// </summary>
    public class HomeGuestController : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<HomeGuestController> _logger;

        /// <summary>
        /// Database context
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// Helper for HomeGuestController
        /// </summary>
        private HomeGuestHelper helper; 

        /// <summary>
        /// Constructor for HomeGuestController class
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="context">Database context</param>
        public HomeGuestController(
            ILogger<HomeGuestController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            db = context;
            helper = new HomeGuestHelper(context);
        }

        /// <summary>
        /// Index GET action
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "HomeAuthorized");
            return View(await helper.GetIndexViewModel());
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
            return View(await helper.GetReviewsViewModel(title, author, groupId, sortOrder, page));
        }

        /// <summary>
        /// ReadReview GET action
        /// </summary>
        /// <param name="reviewId">Review Id</param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> ReadReview(string reviewId)
        {
            return View(await helper.GetReadReviewViewModel(reviewId));
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
