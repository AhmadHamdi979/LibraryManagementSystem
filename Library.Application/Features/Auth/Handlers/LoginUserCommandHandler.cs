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
    class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IAuthService _auth;

        public LoginUserCommandHandler(IAuthService auth)
        {
            _auth = auth;
        }
        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _auth.LoginAsync(request.Request);
        }
    }
}
