using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Course_project.Models.ValidationAttributes
{
    /// <summary>
    /// Attribute for validation user name
    /// </summary>
    public class UserNameValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Allowed characters in user name
        /// </summary>
        private string allowedCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// User name validation
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>bool</returns>
        public override bool IsValid(object value)
        {
            if ((value != null) && 
                ((value as string).
                ToCharArray().Union(allowedCharacters.ToCharArray()).ToArray().Length == allowedCharacters.Length))
                return true;

            return false;
        }
    }
}
