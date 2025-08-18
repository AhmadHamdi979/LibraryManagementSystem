using AutoMapper.QueryableExtensions;
using Library.Application.DTOs.Author;
using Library.Application.Interfaces.Repositories;
using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Persistence.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Authors
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)                
                .ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(Guid id)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task Update(Author author)
        {
            _context.Update(author);
        }
    }
}
