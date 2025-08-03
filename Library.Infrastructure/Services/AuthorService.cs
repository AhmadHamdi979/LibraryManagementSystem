using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.Application.DTOs.Author;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Persistence;
using Library.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Library.Infrastructure.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(AppDbContext context, IMapper mapper, ILogger<AuthorService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
                

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching authors");


            if (pageNumber < 1 || pageSize < 1)
                throw new BadRequestException("Invalid pagination parameters.");

            var authors = await _context.Authors
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (!authors.Any())            
                _logger.LogWarning("No author found");
                
            return authors;
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting author by ID: {AuthorId}", id);

            var author = await _context.Authors.FindAsync(id);

            if (author is null)
            {
                _logger.LogWarning("Author with ID {AuthorId} not found", id);
                throw new NotFoundException("Author", id);
            }

            return _mapper.Map<AuthorDto>(author);
        }
        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorRequest request)
        {
            _logger.LogInformation("Creating a new author: {AuthorName}", request.FullName);

            if (request is null)
                throw new BadRequestException("Request cannot be null");

            var author = _mapper.Map<Author>(request);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return _mapper.Map<AuthorDto>(author);
        }


        public async Task UpdateAuthorAsync(UpdateAuthorRequest request)
        {
            _logger.LogInformation("Updating author with ID: {AuthorId}", request.Id);

            if (request is null)
                throw new BadRequestException("Request cannot be null");

            var author = await _context.Authors.FindAsync(request.Id);

            if (author is null)
            {
                _logger.LogWarning("Attempted to update non-existent author: {AuthorId}", request.Id);
                throw new NotFoundException("Author", request.Id);
            }
            _mapper.Map(request, author);
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
                    
        }

        public async Task DeleteAuthorAsync(Guid id)
        {
            _logger.LogInformation("Deleting author with ID: {AuthorId}", id);

            var author = await _context.Authors.FindAsync(id);

            if (author is null)
            {
                _logger.LogWarning("Attempted to delete non-existent author: {AuthorId}", id);
                throw new NotFoundException("Book", id);
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            
        }
    }
}
