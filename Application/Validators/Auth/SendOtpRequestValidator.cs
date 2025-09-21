using Application.DTOs.Auth.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Auth
{
    public sealed class SendOtpRequestValidator : AbstractValidator<SendOtpRequest>
    {
        public SendOtpRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required to send otp to email!")
                .EmailAddress().WithMessage("Invalid email format!");
        }
    }
}
