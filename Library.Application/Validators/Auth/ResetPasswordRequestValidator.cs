using FluentValidation;
using Library.Application.DTOs.Auth;
using Library.Shared.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Validators.Auth
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer["NullFields"])
                .EmailAddress().WithMessage(localizer["InvalidEmail"]);

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage(localizer["NullFields"]);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(localizer["NullFields"])
                .MinimumLength(6).WithMessage(localizer["PasswordTooShort"]);
        }
    }
}
