using Library.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Auth.Commands
{
    class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IAuthService _auth;

        public RegisterUserCommandHandler(IAuthService auth)
        {
            _auth = auth;
        }
        public Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return _auth.RegisterAsync(request.Request);            
        }
    }
}
