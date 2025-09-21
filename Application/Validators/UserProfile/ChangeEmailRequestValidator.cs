using Application.DTOs.UserProfile.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.UserProfile
{
    public sealed class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
    {
        public ChangeEmailRequestValidator()
        {
            RuleFor(x => x.NewEmail)
                .NotEmpty().WithMessage("You need to provide a new email address.")
                .EmailAddress().WithMessage("The email address provided is not valid.");
        }
    }
}
