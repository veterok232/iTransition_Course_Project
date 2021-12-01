﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_project.Models
{
    public class UserValidator : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            /*if (user.UserName.Contains("admin"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Login must not contain 'admin'"
                });
            }*/
            return Task.FromResult(errors.Count == 0 ?
               IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
