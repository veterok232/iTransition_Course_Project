using Course_project.Models.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Login required!")]
        [Display(Name = "Login")]
        [UserNameValidation(ErrorMessage = "Login contains not allowed characters!")]
        public string Login { get; set; }

        [Required(ErrorMessage = "E-mail required!")]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nickname required!")]
        [Display(Name = "Nickname")]
        [DataType(DataType.Text)]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Password required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords are not equal!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }
    }
}
