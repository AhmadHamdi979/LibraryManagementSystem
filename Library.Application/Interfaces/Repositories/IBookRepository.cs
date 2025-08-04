using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id);
        Task<List<Book>> GetAllAsync(int pageNumber, int pageSize, string? title = null, Guid? authorId = null);
        Task AddAsync(Book book);
        void Remove(Book book);
        Task<bool> ExistsAsync(Guid id);
        Task SaveChangesAsync();
        Task Update(Book book);
    }
}
