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
    public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorRequestValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage(localizer["AuthorFullNameRequired"])
                .MaximumLength(100)
                .WithMessage(localizer["AuthorFullNameMaxLength"]);
        }
    }
}
