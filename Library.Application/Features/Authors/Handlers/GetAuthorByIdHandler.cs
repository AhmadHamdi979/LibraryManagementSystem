using Library.Application.DTOs.Author;
using Library.Application.Features.Authors.Queries;
using Library.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Authors.Handlers
{
    public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
    {
        private readonly IAuthorService _authorService;

        public GetAuthorByIdHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _authorService.GetAuthorByIdAsync(request.Id);
        }
    }
}
