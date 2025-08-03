using Library.Application.DTOs.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Books.Queries
{
    public record GetAllBooksQuery(int pageNumber, int pageSize, string? Title = null, Guid? AuthorId = null) : IRequest<IEnumerable<BookDto>>;
    
    
}
