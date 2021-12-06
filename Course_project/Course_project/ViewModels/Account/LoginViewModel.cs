using System;
using System.ComponentModel.DataAnnotations;

namespace Course_project.ViewModels.Account
{
    /// <summary>
    /// View model for Login page
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Login
        /// </summary>
        [Required(ErrorMessage = "Login required!")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Password required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Remember me
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// Return URL
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
