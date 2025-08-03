using Library.Application.DTOs.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<AuthorDto> CreateAuthorAsync(CreateAuthorRequest request);
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(int pageNumber, int pageSize);
        Task<AuthorDto> GetAuthorByIdAsync(Guid id);
        Task UpdateAuthorAsync(UpdateAuthorRequest request);
        Task DeleteAuthorAsync(Guid id);
    }
}
