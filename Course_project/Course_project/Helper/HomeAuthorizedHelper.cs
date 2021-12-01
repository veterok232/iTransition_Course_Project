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
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Course_project.CloudStorage;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Course_project.Helper
{
    internal static class HomeAuthorizedHelper
    {
        internal static async Task<IndexViewModel> GetIndexViewModel(UserManager<User> userManager, IIdentity userIdentity)
        {
            var user = await userManager.FindByNameAsync(userIdentity.Name);
            return new IndexViewModel() { Nickname = user.Nickname };
        }

        internal static async Task<ReviewViewModel> GetReviewViewModel(
            UserManager<User> userManager,
            IIdentity userIdentity,
            ApplicationDbContext db)
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
                ReviewTags = new List<ReviewTag>()
            };
        }

        internal static async Task UploadFile(ReviewImage reviewImage, ICloudStorage cloudStorage)
        {
            string fileNameForStorage = CreateFileName(reviewImage.ImageFile.FileName);
            reviewImage.ImageUrl = await cloudStorage.UploadFileAsync(reviewImage.ImageFile, fileNameForStorage);
            reviewImage.ImageStorageName = fileNameForStorage;
        }

        private static string CreateFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var fileNameForStorage = $"{fileNameWithoutExt}-{DateTime.Now.ToString("yyyyMMddHHmmss")}{fileExtension}";
            return fileNameForStorage;
        }

        private static List<SelectListItem> CreateReviewGroupsList(List<ReviewGroup> reviewGroups)
        {
            var result = new List<SelectListItem>();
            foreach (var reviewGroup in reviewGroups)
            {
                result.Add(new SelectListItem() { Text = reviewGroup.Name, Value = reviewGroup.Id.ToString() });
            }
            return result;
        }

        private static List<SelectListItem> CreateReviewMarksList()
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
