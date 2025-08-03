using Library.Application.DTOs.Book;
using Library.Application.Features.Books.Queries;
using Library.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Books.Handlers
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookService _bookService;

        public GetAllBooksQueryHandler(IBookService bookService)
        {
            _bookService = bookService;
        }
        public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetAllBooksAsync(request.pageNumber, request.pageSize, request.Title , request.AuthorId);
        }
    }
}
