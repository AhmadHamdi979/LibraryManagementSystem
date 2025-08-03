using Library.Application.Features.Authors.Commands;
using Library.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Authors.Handlers
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, bool>
    {
        private readonly IAuthorService _authorSerive;

        public UpdateAuthorCommandHandler(IAuthorService authorSerive)
        {
            _authorSerive = authorSerive;
        }
        public async Task<bool> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            await _authorSerive.UpdateAuthorAsync(request.Request);
            return true;
        }
    }
}
