using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.Application.DTOs.Book;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Persistence;
using Library.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(AppDbContext context, IMapper mapper, ILogger<BookService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize, string? title = null, Guid? authorId = null)
        {
            _logger.LogInformation("Getting books with filters. Page: {PageNumber}, PageSize: {PageSize}, Title: {Title}, AuthorId: {AuthorId}",
                pageNumber, pageSize, title, authorId);

            if (pageNumber < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid pagination parameters. Page: {PageNumber}, PageSize: {PageSize}", pageNumber, pageSize);
                throw new BadRequestException("Invalid pagination parameters.");
            }

            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(b => b.Title.Contains(title));
                _logger.LogInformation("Applied title filter: {Title}", title);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
                _logger.LogInformation("Applied author filter: {AuthorId}", authorId);
            }

            var books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();                     

            return books;
        }


        public async Task<BookDto?> GetBookByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching book by ID: {BookId}", id);

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                _logger.LogWarning("Book not found with ID: {BookId}", id);
                throw new NotFoundException($"Book with ID {id} was not found.", id);
            }

            _logger.LogInformation("Found book with ID: {BookId}", id);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> CreateBookAsync(CreateBookRequest request)
        {
            _logger.LogInformation("Creating a new book");

            if (request == null)
            {
                _logger.LogWarning("CreateBook request was null");
                throw new BadRequestException("Request cannot be null");
            }

            var authorExist = await _context.Authors.AnyAsync(a => a.Id == request.AuthorId);
            if (!authorExist)
            {
                _logger.LogWarning("Author not found with ID: {AuthorId}", request.AuthorId);
                throw new NotFoundException($"Author with ID {request.AuthorId} was not found.", request.AuthorId);
            }

            var book = _mapper.Map<Book>(request);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new book with ID: {BookId}, Title: {Title}", book.Id, book.Title);

            return _mapper.Map<BookDto>(book);
        }

        public async Task UpdateBookAsync(UpdateBookRequest request)
        {
            _logger.LogInformation("Updating book with ID: {BookId}", request?.Id);

            if (request == null)
            {
                _logger.LogWarning("UpdateBook request was null");
                throw new BadRequestException("Request cannot be null");
            }

            var book = await _context.Books.FindAsync(request.Id);

            if (book == null)
            {
                _logger.LogWarning("Book not found with ID: {BookId}", request.Id);
                throw new NotFoundException($"Book with ID {request.Id} was not found.", request.Id);
            }

            _mapper.Map(request, book);
            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated book with ID: {BookId}", book.Id);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete book with ID: {BookId}", id);

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                _logger.LogWarning("Book not found with ID: {BookId}", id);
                throw new NotFoundException($"Book with ID {id} was not found.", id);
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted book with ID: {BookId}", id);
        }
    }
}
