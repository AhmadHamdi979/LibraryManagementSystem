using AutoMapper;
using AutoMapper.QueryableExtensions;
using Library.Application.DTOs.Author;
using Library.Application.Interfaces;
using Library.Application.Interfaces.Repositories;
using Library.Domain.Entities;
using Library.Persistence;
using Library.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Library.Infrastructure.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthorService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
                

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching authors");


            if (pageNumber < 1 || pageSize < 1)
                throw new BadRequestException("Invalid pagination parameters.");

            var authors = await _unitOfWork.Authors.GetAllAsync(pageNumber, pageSize);

            if (!authors.Any())            
                _logger.LogWarning("No author found");

            var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authors);

            return authorsDto;
        }

        public async Task<AuthorDto> GetAuthorByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting author by ID: {AuthorId}", id);

            var author = await _unitOfWork.Authors.GetByIdAsync(id);

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
            await _unitOfWork.Authors.Update(author);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AuthorDto>(author);
        }


        public async Task UpdateAuthorAsync(UpdateAuthorRequest request)
        {
            _logger.LogInformation("Updating author with ID: {AuthorId}", request.Id);

            if (request is null)
                throw new BadRequestException("Request cannot be null");

            var author = await _unitOfWork.Authors.GetByIdAsync(request.Id);

            if (author is null)
            {
                _logger.LogWarning("Attempted to update non-existent author: {AuthorId}", request.Id);
                throw new NotFoundException("Author", request.Id);
            }
            _mapper.Map(request, author);

            await _unitOfWork.Authors.Update(author);
            await _unitOfWork.SaveChangesAsync();
                    
        }

        public async Task DeleteAuthorAsync(Guid id)
        {
            _logger.LogInformation("Deleting author with ID: {AuthorId}", id);

            var author = await _unitOfWork.Authors.GetByIdAsync(id);

            if (author is null)
            {
                _logger.LogWarning("Attempted to delete non-existent author: {AuthorId}", id);
                throw new NotFoundException("Book", id);
            }
            await _unitOfWork.Authors.Update(author);
            await _unitOfWork.SaveChangesAsync();
            
        }
    }
}
