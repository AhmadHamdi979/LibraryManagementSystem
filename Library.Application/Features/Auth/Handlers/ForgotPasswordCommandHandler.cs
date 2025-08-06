using Library.Application.Features.Auth.Commands;
using Library.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Auth.Handlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, string>
    {
        private readonly IAuthService _authService;

        public ForgotPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public Task<string> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return _authService.GenerateResetPasswordTokenAsync(request.Request.Email);
        }
    }
}
