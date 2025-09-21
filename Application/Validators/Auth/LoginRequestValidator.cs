using Application.DTOs.Auth.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Auth
{
    public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
                .MaximumLength(20).WithMessage("Username must not exceed 30 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required!")
                .NotNull().WithMessage("Password is required!");
        }
    }
}
