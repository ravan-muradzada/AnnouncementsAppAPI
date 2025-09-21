using Application.DTOs.UserProfile.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.UserProfile
{
    public sealed class VerifyEmailChangeRequestValidator : AbstractValidator<VerifyEmailChangeRequest>
    {
        public VerifyEmailChangeRequestValidator()
        {
            RuleFor(x => x.OTP)
                .NotEmpty().WithMessage("OTP is required");
        }
    }
}
