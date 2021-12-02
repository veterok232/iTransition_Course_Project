using Course_project.Models;
using Course_project.ViewModels.HomeAuthorized;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Course_project.Helper;
using Course_project.CloudStorage;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Course_project.Controllers
{
    [Authorize]
    public class HomeAuthorizedController : Controller
    {
        public const int PAGE_SIZE = 2;

        private ApplicationDbContext db;

        private readonly UserManager<User> userManager;

        private readonly ICloudStorage cloudStorage;

        private readonly ILogger<HomeAuthorizedController> _logger;

        public HomeAuthorizedController(
            ILogger<HomeAuthorizedController> logger, 
            UserManager<User> userManager, 
            ApplicationDbContext context,
            ICloudStorage cloudStorage)
        {
            _logger = logger;
            this.userManager = userManager;
            db = context;
            this.cloudStorage = cloudStorage;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var viewModel = await HomeAuthorizedHelper.GetIndexViewModel(userManager, User.Identity);
            return View(viewModel);
        }

        [HttpGet]
        [Route("CreateReview")]
        public async Task<IActionResult> CreateReview()
        {
            var viewModel = await HomeAuthorizedHelper.GetViewModel(userManager, User.Identity, db);
            return View(viewModel);
        }

        [HttpPost]
        [Route("CreateReview")]
        public async Task<IActionResult> CreateReview(
            ReviewViewModel viewModel)
        {
            string reviewImageUrl = await AddReviewImagesToDb(viewModel.ReviewImagesFiles);
            if (reviewImageUrl != null)
            {
                viewModel.Review.ImageUrl = reviewImageUrl;
            }
            await AddReviewToDb(viewModel.Review);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "HomeAuthorized");
        }

        [HttpGet]
        public async Task<IActionResult> Reviews(string title, string author, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            return View(await GetReviewsViewModel(title, author, groupId, sortOrder, page));
        }

        private async Task<string> AddReviewImagesToDb(IFormFileCollection uploads)
        {
            var reviewImages = new List<ReviewImage>();
            if (uploads != null)
            {
                foreach (var uploadedFile in uploads)
                {
                    var reviewImage = new ReviewImage() { ImageFile = uploadedFile };
                    await HomeAuthorizedHelper.UploadFile(reviewImage, cloudStorage);
                    reviewImages.Add(reviewImage);
                }
            }
            db.ReviewImages.AddRange(reviewImages);
            return reviewImages.Count > 0 ? reviewImages[0].ImageUrl : null;
        }

        private async Task AddReviewToDb(Review review)
        {
            User user = GetUserByName(User.Identity.Name);
            review.AuthorId = user.Id;
            review.AuthorNickname = user.Nickname;
            review.AuthorUserName = user.UserName;
            review.PublicationDate = DateTime.Now;
            await db.Reviews.AddAsync(review);
        }

        private User GetUserByName(string userName)
        {
            Task<User> getUserTask = userManager.FindByNameAsync(userName);
            getUserTask.Wait();
            return getUserTask.Result;
        }

        private async Task<ReviewsViewModel> GetReviewsViewModel(string title, string author, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            var viewModel = new ReviewsViewModel();
            viewModel.UserName = User.Identity.Name;
            viewModel.UserNickname = GetUserByName(User.Identity.Name).Nickname;
            var reviewGroupsList = db.ReviewGroups.ToList();
            foreach (var reviewGroup in reviewGroupsList)
            {
                viewModel.ReviewGroups.Add(reviewGroup.Id, reviewGroup.Name);
            }

            //viewModel.Reviews = db.Reviews.ToList();

            //IQueryable<Review> reviews = db.Reviews.Include(x => x.Title).Include(x => x.AuthorUserName);
            IQueryable<Review> reviews = db.Reviews;
            if (!String.IsNullOrEmpty(title))
            {
                reviews = reviews.Where(p => p.Title.Contains(title));
            }
            if (!String.IsNullOrEmpty(author))
            {
                reviews = reviews.Where(p => p.AuthorUserName.Contains(author));
            }
            if (groupId != 0)
            {
                reviews = reviews.Where(p => p.GroupId == groupId);
            }

            switch (sortOrder)
            {
                case SortState.AuthorAsc:
                    reviews = reviews.OrderBy(s => s.AuthorUserName);
                    break;
                case SortState.AuthorDesc:
                    reviews = reviews.OrderByDescending(s => s.AuthorUserName);
                    break;
                case SortState.PublicationDateAsc:
                    reviews = reviews.OrderBy(s => s.PublicationDate);
                    break;
                case SortState.PublicationDateDesc:
                    reviews = reviews.OrderByDescending(s => s.PublicationDate);
                    break;
                /*case SortState.RatingAsc:
                    reviews = reviews.OrderBy(s => s.Rating);
                    break;
                case SortState.RatingDesc:
                    reviews = reviews.OrderByDescending(s => s.Rating);
                    break;*/
                case SortState.TitleAsc:
                    reviews = reviews.OrderBy(s => s.Title);
                    break;
                case SortState.TitleDesc:
                    reviews = reviews.OrderByDescending(s => s.Title);
                    break;
                default:
                    reviews = reviews.OrderByDescending(s => s.PublicationDate);
                    break;
            }

            var count = await reviews.CountAsync();
            var items = await reviews.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToListAsync();

            viewModel.FilterViewModel = GetFilterViewModel(viewModel.ReviewGroups.Values.ToList(), groupId, title, author);
            viewModel.SortViewModel = GetSortViewModel(sortOrder);
            viewModel.PageViewModel = GetPageViewModel(count, page, PAGE_SIZE);
            viewModel.Reviews = items;
            return viewModel;
        }

        private FilterViewModel GetFilterViewModel(List<string> reviewGroups, int selectedGroupId, string title, string author)
        {
            reviewGroups.Insert(0, "All");
            var items = new List<SelectListItem>();
            for (int i = 0; i < reviewGroups.Count; i++)
            {
                items.Add(new SelectListItem(reviewGroups[i], i.ToString()));
            }
            items[selectedGroupId].Selected = true;
            return new FilterViewModel() 
            { 
                ReviewGroupsSelect = items,
                TitleFilter = title,
                AuthorFilter = author,
                SelectedGroup = selectedGroupId };
        }

        private SortViewModel GetSortViewModel(SortState sortOrder)
        {
            var viewModel = new SortViewModel(SortState.PublicationDateDesc)
            {
                SortBySelect = new List<SelectListItem>()
                {
                    new SelectListItem("By title asc", SortState.TitleAsc.ToString()),
                    new SelectListItem("By title desc", SortState.TitleDesc.ToString()),
                    new SelectListItem("By author asc", SortState.AuthorAsc.ToString()),
                    new SelectListItem("By author desc", SortState.AuthorDesc.ToString()),
                    new SelectListItem("By rating asc", SortState.RatingAsc.ToString()),
                    new SelectListItem("By rating desc", SortState.RatingDesc.ToString()),
                    new SelectListItem("By publication date asc", SortState.PublicationDateAsc.ToString()),
                    new SelectListItem("By publication date desc", SortState.PublicationDateDesc.ToString())
                },
                CurrentSort = sortOrder
            };
            viewModel.SortBySelect[(int)sortOrder].Selected = true;
            return viewModel;
        }

        private PageViewModel GetPageViewModel(int count, int page, int pageSize)
        {
            return new PageViewModel(count, page, pageSize);
        }

        /*private async Task UploadFileToGoogle(ReviewImage reviewImage)
        {
            string fileNameForStorage = FormFileName(reviewImage.ImageFile.FileName);
            reviewImage.ImageUrl = await cloudStorage.UploadFileAsync(reviewImage.ImageFile, fileNameForStorage);
            reviewImage.ImageStorageName = fileNameForStorage;
        }*/

        /* private static string FormFileName(string fileName)
         {
             var fileExtension = Path.GetExtension(fileName);
             var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
             var fileNameForStorage = $"{fileNameWithoutExt}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
             return fileNameForStorage;
         }*/

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
