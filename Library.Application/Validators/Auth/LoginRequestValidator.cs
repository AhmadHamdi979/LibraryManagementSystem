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
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator(IStringLocalizer<SharedResource> localizer) 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(localizer["NullFields"])
                .EmailAddress().WithMessage(localizer["InvalidEmail"]);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(localizer["NullFields"]);
        }
    }
}
