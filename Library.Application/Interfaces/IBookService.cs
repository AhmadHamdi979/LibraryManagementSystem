using Library.Application.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize, string? title, Guid? id);
        Task<BookDto?> GetBookByIdAsync(Guid id);
        Task<BookDto> CreateBookAsync(CreateBookRequest request);
        Task UpdateBookAsync(UpdateBookRequest request);
        Task DeleteBookAsync(Guid id);
    }
}
