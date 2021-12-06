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
    public class HomeGuestController : Controller
    {
        private readonly ILogger<HomeGuestController> _logger;

        private ApplicationDbContext db;

        private HomeGuestHelper helper; 

        public HomeGuestController(
            ILogger<HomeGuestController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            db = context;
            helper = new HomeGuestHelper(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "HomeAuthorized");
            return View(await helper.GetIndexViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Reviews(string title, string author, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            return View(await helper.GetReviewsViewModel(title, author, groupId, sortOrder, page));
        }

        public async Task<IActionResult> ReadReview(string reviewId)
        {
            return View(await helper.GetReadReviewViewModel(reviewId));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
