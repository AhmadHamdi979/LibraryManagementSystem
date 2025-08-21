using FluentValidation;
using Library.Application.DTOs.Author;
using Library.Shared.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Validators.Authors
{
    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(localizer["AuthorIdRequired"]);

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage(localizer["AuthorFullNameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["AuthorFullNameMaxLength"]);
        }
    }
}
