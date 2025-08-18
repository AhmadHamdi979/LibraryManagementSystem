using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository Authors { get; }
        IAuthRepository Users { get; }
        IBookRepository Books { get; }
        Task<int> SaveChangesAsync();
    }
}
