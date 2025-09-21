using Application.DTOs.Announcement.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Announcement
{
    public sealed class UpdateAnnouncementRequestValidator : AbstractValidator<UpdateAnnouncementRequest>
    {
        public UpdateAnnouncementRequestValidator()
        {
            RuleFor(x => x.Title)
                .Length(5, 20);

            RuleFor(x => x.Content)
                .Length(20, 1000);

            RuleFor(x => x.Category)
                .Must(c => new[] { "News", "Event", "Alert" }.Contains(c));

            RuleFor(x => x.ExpiresAt)
                .Must(date => date is null || date > DateTime.UtcNow);
        }
    }
}
