using Application.DTOs.Auth.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Auth
{
    public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required to register!")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(30).WithMessage("Username must not exceed 30 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required to register!")
                .EmailAddress().WithMessage("A valid email is required to register.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required to register!");
        }
    }
}
