using Course_project.Helper;
using Course_project.Models;
using Course_project.Services;
using Course_project.ViewModels.Account;
using Course_project.ViewModels.ReviewsFilterSortPagination;
using Course_project.ViewModels.UsersFilterSortPagination;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Course_project.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;

        private readonly SignInManager<User> signInManager;
        
        private readonly RoleManager<IdentityRole> roleManager;

        private ApplicationDbContext db;

        private AccountHelper helper;

        private DatabaseInteractionHelper databaseHelper;

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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

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

        [AllowAnonymous]
        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        [AllowAnonymous]
        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            return await ExternalLogin();
        }

        [AllowAnonymous]
        [Route("facebook-login")]
        public IActionResult FacebookLogin()
        {
            string redirectUrl = Url.Action("FacebookResponse", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult(FacebookDefaults.AuthenticationScheme, properties);
        }

        [AllowAnonymous]
        [Route("facebook-response")]
        public async Task<IActionResult> FacebookResponse()
        {
            return await ExternalLogin();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "HomeGuest");
        }

        [HttpGet]
        public async Task<IActionResult> UserPage(string userId, string title, int groupId,
            SortState sortOrder = SortState.PublicationDateDesc, int page = 1)
        {
            return View(await helper.GetUserPageViewModel(userId, title, groupId, sortOrder, page));
        }

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

        [Authorize(Roles="admin")]
        public async Task<IActionResult> AdminAccount(string userId, string userName,
            UsersSortState sortOrder = UsersSortState.UserNameAsc, int page = 1)
        {
            return View(await helper.GetAdminAccountViewModel(userId, userName, sortOrder, page));
        }

        private async Task SendRegistrationEmail(RegisterViewModel model)
        {
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(model.Email, "Thanks for registration!", $"<p>Welcome to my site!<p><p>Your login: {model.Login}</p><p>Your password: {model.Password}</p>");
        }

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
