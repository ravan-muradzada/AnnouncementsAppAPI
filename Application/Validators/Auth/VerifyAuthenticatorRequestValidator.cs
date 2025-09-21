using Application.DTOs.Auth.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Auth
{
    public sealed class VerifyAuthenticatorRequestValidator : AbstractValidator<VerifyAuthenticatorRequest>
    {
        public VerifyAuthenticatorRequestValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("You need to attach the code to authenticate!");
        }
    }
}
