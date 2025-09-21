using Application.DTOs.Auth.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Auth
{
    public sealed class VerifyTwoFactorAuthRequestValidator : AbstractValidator<VerifyTwoFactorAuthRequest>
    {
        public VerifyTwoFactorAuthRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("You need to enter your email to verify 2FA!")
                .EmailAddress().WithMessage("You need to enter a valid email address to verify 2FA!");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("You need to enter the code to verify 2FA!");
        }
    }
}
