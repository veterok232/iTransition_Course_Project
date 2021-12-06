using Course_project.Models.ValidationAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Course_project.ViewModels.Account
{
    /// <summary>
    /// View model for Register page
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Login
        /// </summary>
        [Required(ErrorMessage = "Login required!")]
        [Display(Name = "Login")]
        [UserNameValidation(ErrorMessage = "Login contains not allowed characters!")]
        public string Login { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "E-mail required!")]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Nickname
        /// </summary>
        [Required(ErrorMessage = "Nickname required!")]
        [Display(Name = "Nickname")]
        [DataType(DataType.Text)]
        public string Nickname { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Password required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Password confirm
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Passwords are not equal!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}
