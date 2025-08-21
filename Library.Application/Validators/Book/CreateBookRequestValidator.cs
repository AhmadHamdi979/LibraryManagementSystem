using FluentValidation;
using Library.Application.DTOs.Book;
using Library.Shared.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Validators.Book
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(localizer["BookTitleRequired"])
                .MaximumLength(200)
                .WithMessage(localizer["BookTitleMaxLength"]);

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(localizer["BookDescriptionRequired"])
                .MaximumLength(1000)
                .WithMessage(localizer["BookDescriptionMaxLength"]);

            RuleFor(x => x.PublishDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage(localizer["BookPublishDateInvalid"]);

            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .WithMessage(localizer["BookAuthorIdRequired"]);
        }
    }
}
