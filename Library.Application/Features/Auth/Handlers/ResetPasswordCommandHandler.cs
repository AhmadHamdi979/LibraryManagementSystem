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
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IAuthService _authService;

        public ResetPasswordCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _authService.ResetPasswordAsync(
                request.Request.Email,
                request.Request.Token,
                request.Request.NewPassword);

            return Unit.Value;
        }
    }
}
