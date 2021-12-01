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

namespace Course_project.Controllers
{
    [Authorize]
    public class HomeAuthorizedController : Controller
    {
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
            var viewModel = await HomeAuthorizedHelper.GetReviewViewModel(userManager, User.Identity, db);
            return View(viewModel);
        }

        [HttpPost]
        [Route("CreateReview")]
        public async Task<IActionResult> CreateReview(
            ReviewViewModel viewModel)
        {
            var uploads = viewModel.ReviewImagesFiles;
            var reviewImages = new List<ReviewImage>();
            foreach (var uploadedFile in uploads)
            {
                var reviewImage = new ReviewImage() { ImageFile = uploadedFile };
                await HomeAuthorizedHelper.UploadFile(reviewImage, cloudStorage);
                reviewImages.Add(reviewImage);
            }
            db.ReviewImages.AddRange(reviewImages);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "HomeAuthorized");
        }

        [HttpGet]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(
            [Bind("Id,ReviewId,ImageUrl,ImageFile,ImageStorageName,ImageFileName")]
            ReviewImage reviewImage)
        {
            if (reviewImage.ImageFile != null)
            {
                await UploadFileToGoogle(reviewImage);
            }

            db.ReviewImages.Add(reviewImage);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "HomeAuthorized");
        }

        private async Task UploadFileToGoogle(ReviewImage reviewImage)
        {
            string fileNameForStorage = FormFileName(reviewImage.ImageFile.FileName);
            reviewImage.ImageUrl = await cloudStorage.UploadFileAsync(reviewImage.ImageFile, fileNameForStorage);
            reviewImage.ImageStorageName = fileNameForStorage;
        }

        private static string FormFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var fileNameForStorage = $"{fileNameWithoutExt}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
