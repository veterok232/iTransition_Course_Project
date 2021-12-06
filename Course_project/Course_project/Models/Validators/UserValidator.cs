using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Course_project.Models
{
    /// <summary>
    /// Class for User model validation
    /// </summary>
    public class UserValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            return Task.FromResult(errors.Count == 0 ?
               IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
