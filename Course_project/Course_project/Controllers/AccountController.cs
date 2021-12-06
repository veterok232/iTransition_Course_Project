using Course_project.Helper;
using Course_project.Models;
using Course_project.Services;
using Course_project.ViewModels.Account;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using Course_project.ViewModels.UsersFilterSortPagination;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Course_project.Controllers
{
    /// <summary>
    /// Controller for user accounts
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
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
        /// Helper class for Account controller
        /// </summary>
        private AccountHelper helper;

        /// <summary>
        /// Helper for database interactions
        /// </summary>
        private DatabaseInteractionHelper databaseHelper;

        /// <summary>
        /// Constructor of AccountController class
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <param name="signInManager">Sign in manager</param>
        /// <param name="roleManager">Role manager</param>
        /// <param name="context">Database context</param>
        public AccountController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            db = context;
            helper = new AccountHelper(userManager, signInManager, roleManager, context);
            databaseHelper = new DatabaseInteractionHelper(context, userManager);
        }

        /// <summary>
        /// Register GET action
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register POST action
        /// </summary>
        /// <param name="model">RegisterViewModel object</param>
        /// <returns>Task<IActionResult></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Login, Nickname = model.Nickname, Email = model.Email };
                IdentityResult result = null;
                try
                {
                    if (await databaseHelper.GetUserByEmailAsync(model.Email) != null)
                    {
                        ModelState.AddModelError(string.Empty, "This e-mail has already been registered!");
                    }
                    result = await userManager.CreateAsync(user, model.Password);
                }
                catch 
                {
                    ModelState.AddModelError(string.Empty, "This user has already registered!");
                    return View(model);
                }
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                    await signInManager.SignInAsync(user, false);
                    await SendRegistrationEmail(model);
                    return RedirectToAction("Index", "HomeAuthorized");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        /// <summary>
        /// Login GET action
        /// </summary>
        /// <param name="returnUrl">Return URL</param>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Login POST action
        /// </summary>
        /// <param name="model">LoginViewModel</param>
        /// <returns>Task<IActionResult></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "HomeAuthorized");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong login or(and) password!");
                }
            }
            return View(model);
        }

        /// <summary>
        /// Google login action
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        /// <summary>
        /// Response from Google server
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [AllowAnonymous]
        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            return await ExternalLogin();
        }

        /// <summary>
        /// Facebook login action
        /// </summary>
        /// <returns>IActionResult</returns>
        [AllowAnonymous]
        [Route("facebook-login")]
        public IActionResult FacebookLogin()
        {
            string redirectUrl = Url.Action("FacebookResponse", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult(FacebookDefaults.AuthenticationScheme, properties);
        }

        /// <summary>
        /// Response from Facebook server
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [AllowAnonymous]
        [Route("facebook-response")]
        public async Task<IActionResult> FacebookResponse()
        {
            return await ExternalLogin();
        }

        /// <summary>
        /// Logout action
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "HomeGuest");
        }

        /// <summary>
        /// Show user page GET action
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="title">Title</param>
        /// <param name="groupId">GroupId</param>
        /// <param name="sortOrder">Sort order</param>
        /// <param name="page">Page</param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> UserPage(string userId, string title, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            return View(await helper.GetUserPageViewModel(userId, title, groupId, sortOrder, page));
        }

        /// <summary>
        /// Show user personal account GET action
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="groupId">Group Id</param>
        /// <param name="sortOrder">Sort order</param>
        /// <param name="page">Page</param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> PersonalAccount(string title, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            if (User.Identity.Name == "admin")
            {
                return RedirectToAction("AdminAccount", "Account");
            }
            else
            {
                var userId = (await databaseHelper.GetUserByNameAsync(User.Identity.Name)).Id;
                return View(await helper.GetPersonalAccountViewModel(userId, title, groupId, sortOrder, page));
            }
        }

        /// <summary>
        /// Show admin account GET action
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="userName">User name</param>
        /// <param name="sortOrder">Sort order</param>
        /// <param name="page">Page</param>
        /// <returns>Task<IActionResult></returns>
        [Authorize(Roles="admin")]
        [HttpGet]
        public async Task<IActionResult> AdminAccount(string userId, string userName,
            UsersSortState sortOrder = UsersSortState.UserNameAsc, int page = 1)
        {
            return View(await helper.GetAdminAccountViewModel(userId, userName, sortOrder, page));
        }

        /// <summary>
        /// Send registration e-mail action
        /// </summary>
        /// <param name="model">RegisterViewModel</param>
        /// <returns>Task</returns>
        private async Task SendRegistrationEmail(RegisterViewModel model)
        {
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(model.Email, "Thanks for registration!", $"<p>Welcome to my site!<p><p>Your login: {model.Login}</p><p>Your password: {model.Password}</p>");
        }

        /// <summary>
        /// External login 
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        private async Task<IActionResult> ExternalLogin()
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
                return RedirectToAction("Index", "HomeAuthorized");
            else
            {
                User user = new User { UserName = $"_{userInfo[1]}", Nickname = userInfo[0], Email = userInfo[1] };

                var identityResult = await userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    identityResult = await userManager.AddLoginAsync(user, info);
                    if (identityResult.Succeeded)
                    {
                        var res = await userManager.AddToRoleAsync(user, "user");
                        await signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "HomeAuthorized");
                    }
                }
                return StatusCode(401);
            }
        }
    }
}
