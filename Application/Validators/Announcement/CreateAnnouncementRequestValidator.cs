using Application.DTOs.Announcement.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Announcement
{
    public sealed class CreateAnnouncementRequestValidator : AbstractValidator<CreateAnnouncementRequest>
    {
        public CreateAnnouncementRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required to create announcement!")
                .Length(5, 20);

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required to create announcement!")
                .Length(20, 1000);

            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Category is required to create announcement!")
                .Must(c => new[] { "News", "Event", "Alert" }.Contains(c));

            RuleFor(x => x.ExpiresAt)
                .Must(date => date is null || date > DateTime.UtcNow);
        }
    }
}
