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
    /// <summary>
    /// Helper class for AccountController
    /// </summary>
    internal class AccountHelper : GeneralHelper
    {
        /// <summary>
        /// Count user per one page on AdminAccount page
        /// </summary>
        internal const int ADMIN_PAGE_SIZE = 10;

        /// <summary>
        /// Count reviews per one page on UserPage page
        /// </summary>
        internal const int USER_PAGE_SIZE = 5;

        /// <summary>
        /// User manager
        /// </summary>
        private readonly UserManager<User> userManager;

        /// <summary>
        /// Sign in manager
        /// </summary>
        private readonly SignInManager<User> signInManager;

        /// <summary>
        /// Role manager
        /// </summary>
        private readonly RoleManager<IdentityRole> roleManager;

        /// <summary>
        /// Database context
        /// </summary>
        private ApplicationDbContext db;

        /// <summary>
        /// Helper for database interactions
        /// </summary>
        private DatabaseInteractionHelper databaseHelper;

        /// <summary>
        /// Constructor for AccountHelper class
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <param name="signInManager">Sign in manager</param>
        /// <param name="roleManager">Role manager</param>
        /// <param name="context">Database context</param>
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

        /// <summary>
        /// Get UserPageViewModel 
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="title">Title</param>
        /// <param name="groupId">Group Id</param>
        /// <param name="sortOrder">Sort order</param>
        /// <param name="page">Page</param>
        /// <returns>Task<UserPageViewModel></returns>
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

        /// <summary>
        /// Get PersonalAccountViewModel
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="title">Title</param>
        /// <param name="groupId">Group Id</param>
        /// <param name="sortOrder">Sort order</param>
        /// <param name="page">Page</param>
        /// <returns>Task<PersonalAccountViewModel></returns>
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

        /// <summary>
        /// Get AdminAccountViewModel
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="userName">User name</param>
        /// <param name="sortOrder">Sort order</param>
        /// <param name="page">Page</param>
        /// <returns>Task<AdminAccountViewModel></returns>
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

        /// <summary>
        /// Get user name from Google and Facebook response
        /// </summary>
        /// <param name="claims">Claims</param>
        /// <returns>string</returns>
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

        /// <summary>
        /// Get UsersFilterViewModel
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="userName">User name</param>
        /// <returns>UsersFilterViewModel</returns>
        internal UsersFilterViewModel GetUsersFilterViewModel(string userId, string userName)
        {
            return new UsersFilterViewModel()
            {
                UserNameFilter = userName,
                UserIdFilter = userId
            };
        }

        /// <summary>
        /// Get UsersSortViewModel
        /// </summary>
        /// <param name="sortOrder">Sort order</param>
        /// <returns>UsersSortViewModel</returns>
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

        /// <summary>
        /// Get UsersPageViewModel
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>UsersPageViewModel</returns>
        internal UsersPageViewModel GetUsersPageViewModel(int count, int page, int pageSize)
        {
            return new UsersPageViewModel(count, page, pageSize);
        }

        /// <summary>
        /// Get users count
        /// </summary>
        /// <param name="users">Users</param>
        /// <returns>Task<int></returns>
        internal async Task<int> GetUsersCount(IQueryable<User> users)
        {
            return await users.CountAsync();
        }

        /// <summary>
        /// Get list of users on current page
        /// </summary>
        /// <param name="users">Users</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Task<List<User>></returns>
        internal async Task<List<User>> GetPageUsers(IQueryable<User> users, int page, int pageSize)
        {
            return await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        /// <summary>
        /// Sort users by sortOrder criteria
        /// </summary>
        /// <param name="users">Users</param>
        /// <param name="sortOrder">Sort order</param>
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

        /// <summary>
        /// Filter users by userId and userName
        /// </summary>
        /// <param name="users">Users</param>
        /// <param name="userId">User Id</param>
        /// <param name="userName">User name</param>
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
