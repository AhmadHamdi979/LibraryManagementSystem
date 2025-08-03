using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Authors.Commands
{
    public record DeleteAuthorCommand(Guid Id) : IRequest<bool>;
    
}
