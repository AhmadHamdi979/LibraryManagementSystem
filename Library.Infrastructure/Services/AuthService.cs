using System;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Application.DTOs.Auth;
using AutoMapper;
using Library.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Library.Application.Interfaces.Repositories;

namespace Library.Infrastructure.Services
{
    class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IAuthRepository authRepository, IMapper mapper, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for {Email}", request.Email);

            if (await _authRepository.EmailExistsAsync(request.Email))
            {
                _logger.LogWarning("Registration failed: Email already used => {Email}", request.Email);
                throw new BadRequestException("Email already used.");
            }

            var user = _mapper.Map<User>(request);
            user.Id = Guid.NewGuid();
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _authRepository.AddAsync(user);
            await _authRepository.SaveChangesAsync();

            return _jwtService.GenerateToken(user);
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);

            var user = await _authRepository.GetByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
            {
                _logger.LogWarning("Login failed for {Email}", request.Email);
                throw new UnauthorizedException("Invalid email or password.");
            }
            return _jwtService.GenerateToken(user);
        }

    }
}
