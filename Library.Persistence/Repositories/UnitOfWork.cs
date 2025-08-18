using Library.Application.Interfaces.Repositories;
using Library.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IAuthorRepository Authors { get; }
        public IAuthRepository Users { get; }
        public IBookRepository Books { get; }

        public UnitOfWork(AppDbContext context, IAuthorRepository authors, IAuthRepository users, IBookRepository books)
        {
            _context = context;
            Authors = authors;
            Users = users;
            Books = books;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
