using Course_project.Models;
using Course_project.ViewModels.Account;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using Course_project.ViewModels.UsersFilterSortPagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Helper
{
    internal class AccountHelper : GeneralHelper
    {
        internal const int ADMIN_PAGE_SIZE = 10;

        internal const int USER_PAGE_SIZE = 5;

        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private ApplicationDbContext db;

        private DatabaseInteractionHelper databaseHelper;

        internal AccountHelper(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            db = context;
            databaseHelper = new DatabaseInteractionHelper(context, userManager);
        }

        internal async Task<UserPageViewModel> GetUserPageViewModel(string userId, string title, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            var viewModel = new UserPageViewModel();
            var user = await databaseHelper.GetUserById(userId);
            viewModel.UserId = userId;
            viewModel.UserName = user.UserName;
            viewModel.UserNickname = user.Nickname;
            viewModel.ReviewGroups = await databaseHelper.GetReviewGroupsAsync();
            IQueryable<Review> reviews = db.Reviews;
            FilterReviews(ref reviews, title, user.UserName, groupId);
            SortReviews(ref reviews, sortOrder);
            viewModel.Reviews = await GetPageReviews(reviews, page, REVIEWS_PAGE_SIZE);
            viewModel.FilterViewModel = GetFilterViewModel(
                viewModel.ReviewGroups.Values.ToList(), groupId, title, user.UserName);
            viewModel.SortViewModel = GetSortViewModel(sortOrder);
            viewModel.PageViewModel = GetPageViewModel(await GetReviewsCount(reviews), page, USER_PAGE_SIZE);

            return viewModel;
        }

        internal async Task<PersonalAccountViewModel> GetPersonalAccountViewModel(string userId, string title, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            var viewModel = new PersonalAccountViewModel();
            var user = await databaseHelper.GetUserById(userId);
            viewModel.UserId = userId;
            viewModel.UserName = user.UserName;
            viewModel.UserNickname = user.Nickname;
            viewModel.ReviewGroups = await databaseHelper.GetReviewGroupsAsync();
            IQueryable<Review> reviews = db.Reviews;
            FilterReviews(ref reviews, title, user.UserName, groupId);
            SortReviews(ref reviews, sortOrder);
            viewModel.Reviews = await GetPageReviews(reviews, page, REVIEWS_PAGE_SIZE);
            viewModel.FilterViewModel = GetFilterViewModel(
                viewModel.ReviewGroups.Values.ToList(), groupId, title, user.UserName);
            viewModel.SortViewModel = GetSortViewModel(sortOrder);
            viewModel.PageViewModel = GetPageViewModel(await GetReviewsCount(reviews), page, USER_PAGE_SIZE);

            return viewModel;
        }

        internal async Task<AdminAccountViewModel> GetAdminAccountViewModel(string userId, string userName,
            UsersSortState sortOrder = UsersSortState.UserNameAsc, int page = 1)
        {
            var viewModel = new AdminAccountViewModel();
            var user = await databaseHelper.GetUserByNameAsync("admin");
            viewModel.UserId = userId;
            viewModel.UserName = user.UserName;
            viewModel.UserNickname = user.Nickname;
            IQueryable<User> users = db.Users;
            FilterUsers(ref users, userId, userName);
            SortUsers(ref users, sortOrder);
            viewModel.Users = await GetPageUsers(users, page, ADMIN_PAGE_SIZE);
            viewModel.FilterViewModel = GetUsersFilterViewModel(userId, userName);
            viewModel.SortViewModel = GetUsersSortViewModel(sortOrder);
            viewModel.PageViewModel = GetUsersPageViewModel(await GetUsersCount(users), page, ADMIN_PAGE_SIZE);

            return viewModel;
        }

        internal string GetUserName(IEnumerable<dynamic> claims)
        {
            foreach (var claim in claims)
            {
                var uri = new Uri(claim.Type);
                if (uri.Segments[uri.Segments.Length - 1] == "givenname")
                    return claim.Value;
            }
            return null;
        }

        internal UsersFilterViewModel GetUsersFilterViewModel(string userId, string userName)
        {
            return new UsersFilterViewModel()
            {
                UserNameFilter = userName,
                UserIdFilter = userId
            };
        }

        internal virtual UsersSortViewModel GetUsersSortViewModel(UsersSortState sortOrder)
        {
            var viewModel = new UsersSortViewModel(UsersSortState.UserNameAsc)
            {
                SortBySelect = new List<SelectListItem>()
                {
                    new SelectListItem("By user name asc", UsersSortState.UserNameAsc.ToString()),
                    new SelectListItem("By user name desc", UsersSortState.UserNameDesc.ToString()),
                    new SelectListItem("By user nickname asc", UsersSortState.UserNicknameAsc.ToString()),
                    new SelectListItem("By user nickname desc", UsersSortState.UserNicknameDesc.ToString()),
                },
                CurrentSort = sortOrder
            };
            viewModel.SortBySelect[(int)sortOrder].Selected = true;

            return viewModel;
        }

        internal UsersPageViewModel GetUsersPageViewModel(int count, int page, int pageSize)
        {
            return new UsersPageViewModel(count, page, pageSize);
        }

        internal async Task<int> GetUsersCount(IQueryable<User> users)
        {
            return await users.CountAsync();
        }

        internal async Task<List<User>> GetPageUsers(IQueryable<User> users, int page, int pageSize)
        {
            return await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        internal void SortUsers(ref IQueryable<User> users, UsersSortState sortOrder)
        {
            switch (sortOrder)
            {
                case UsersSortState.UserNameAsc:
                    users = users.OrderBy(s => s.UserName);
                    break;
                case UsersSortState.UserNameDesc:
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                case UsersSortState.UserNicknameAsc:
                    users = users.OrderBy(s => s.Nickname);
                    break;
                case UsersSortState.UserNicknameDesc:
                    users = users.OrderByDescending(s => s.Nickname);
                    break;
                default:
                    users = users.OrderBy(s => s.UserName);
                    break;
            }
        }

        internal void FilterUsers(ref IQueryable<User> users, string userId, string userName)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                users = users.Where(p => p.Id == userId);
            }
            if (!String.IsNullOrEmpty(userName))
            {
                users = users.Where(p => p.UserName.Contains(userName));
            }
        }
    }
}
