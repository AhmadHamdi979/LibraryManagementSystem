using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Repositories
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAsync(int pageNumber, int pageSize);
        Task<Author?> GetByIdAsync(Guid id);
        Task SaveChangesAsync();
        Task Update(Author author);
    }
}
