using Application.DTOs.Auth.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Auth
{
    public sealed class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("You need to enter your email to reset password.")
                .EmailAddress().WithMessage("You need to enter a valid email address to reset password.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("You need to enter new password to reset password.");
        }
    }
}
