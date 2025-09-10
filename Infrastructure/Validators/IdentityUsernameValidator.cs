using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Validators
{
    public class IdentityUsernameValidator : IUserValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var errors = new List<IdentityError>();

            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "EmptyUserName",
                    Description = "Username cannot be empty."
                });
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            // Regex for Instagram-style usernames
            var regex = new Regex(@"^(?!.*\.\.)(?!.*\.$)[a-zA-Z0-9._]{3,30}$");

            if (!regex.IsMatch(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "InvalidUserName",
                    Description = "Username must be 3-30 characters long and can only contain letters, numbers, underscores, and periods. It cannot end with a period or contain consecutive periods."
                });
            }

            return Task.FromResult(errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success);
        }
    }
}
