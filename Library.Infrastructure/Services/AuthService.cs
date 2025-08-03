using System;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.DTOs.Auth;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Library.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Services
{
    class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, IMapper mapper, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for {Email}", request.Email);

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                _logger.LogWarning("Registration failed: Email already used => {Email}", request.Email);
                throw new BadRequestException("Email already used.");
            }
            var user = _mapper.Map<User>(request);
            user.Id = Guid.NewGuid();
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _jwtService.GenerateToken(user);
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
            {
                _logger.LogWarning("Login failed for {Email}", request.Email);
                throw new UnauthorizedException("Invalid email or password.");
            }
            return _jwtService.GenerateToken(user);
        }

    }
}
