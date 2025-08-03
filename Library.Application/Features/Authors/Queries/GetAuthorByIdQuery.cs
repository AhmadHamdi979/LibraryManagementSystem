using Library.Application.DTOs.Author;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Authors.Queries
{
    public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDto>;
    
}
