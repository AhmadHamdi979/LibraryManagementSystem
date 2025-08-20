using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.Application.DTOs.Book;
using Library.Application.Interfaces;
using Library.Application.Interfaces.Repositories;
using Library.Domain.Entities;
using Library.Persistence;
using Library.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookService> logger)
        {
            _unitOfWork = unitOfWork;
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
                throw new BadRequestException("InvalidPaginationParameters");
            }

            var books = await _unitOfWork.Books.GetAllAsync(pageNumber, pageSize, title, authorId);

            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);

            return booksDto;
        }


        public async Task<BookDto?> GetBookByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching book by ID: {BookId}", id);

            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
            {
                _logger.LogWarning("Book not found with ID: {BookId}", id);
                throw new NotFoundException("BookNotFound",id);
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
                throw new BadRequestException("NullFileds");
            }

            var authorExist = await _unitOfWork.Authors.GetByIdAsync(request.AuthorId);
            if (authorExist == null)
            {
                _logger.LogWarning("Author not found with ID: {AuthorId}", request.AuthorId);
                throw new NotFoundException("AuthorNotFound", request.AuthorId);
            }

            var book = _mapper.Map<Book>(request);
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created new book with ID: {BookId}, Title: {Title}", book.Id, book.Title);

            return _mapper.Map<BookDto>(book);
        }

        public async Task UpdateBookAsync(UpdateBookRequest request)
        {
            _logger.LogInformation("Updating book with ID: {BookId}", request?.Id);

            if (request == null)
            {
                _logger.LogWarning("UpdateBook request was null");
                throw new BadRequestException("NullFileds");
            }

            var book = await _unitOfWork.Books.GetByIdAsync(request.Id);

            if (book == null)
            {
                _logger.LogWarning("Book not found with ID: {BookId}", request.Id);
                throw new NotFoundException("BookNotFound", request.Id);
            }

            _mapper.Map(request, book);
            await _unitOfWork.Books.Update(book);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully updated book with ID: {BookId}", book.Id);
        }

        public async Task DeleteBookAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete book with ID: {BookId}", id);

            var book = await _unitOfWork.Books.GetByIdAsync(id);

            if (book == null)
            {
                _logger.LogWarning("Book not found with ID: {BookId}", id);
                throw new NotFoundException("BookNotFound", id);
            }

            _unitOfWork.Books.Remove(book);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Deleted book with ID: {BookId}", id);
        }
    }
}
