using Library.Application.DTOs.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Features.Books.Commands
{
    public record CreateBookCommand(CreateBookRequest Request) : IRequest<BookDto>;    
    
}
