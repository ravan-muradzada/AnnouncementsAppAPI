using Application.DTOs.UserProfile.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.UserProfile
{
    public sealed class ChangeUsernameRequestValidator : AbstractValidator<ChangeUsernameRequest>
    {
        public ChangeUsernameRequestValidator()
        {
            RuleFor(x => x.NewUsername)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
                .MaximumLength(20).WithMessage("Username must not exceed 30 characters");
        }
    }
}
