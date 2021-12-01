using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models.ValidationAttributes
{
    public class UserNameValidationAttribute : ValidationAttribute
    {
        private string allowedCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

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
