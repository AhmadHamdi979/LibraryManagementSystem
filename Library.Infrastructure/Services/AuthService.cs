using System;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Application.DTOs.Auth;
using AutoMapper;
using Library.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Library.Application.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Library.Infrastructure.Services
{
    class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper,
            IJwtService jwtService, ILogger<AuthService> logger, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jwtService;
            _logger = logger;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for {Email}", request.Email);

            if (await _unitOfWork.Users.EmailExistsAsync(request.Email))
            {
                _logger.LogWarning("Registration failed: Email already used => {Email}", request.Email);
                throw new BadRequestException("Email already used.");
            }

            var user = _mapper.Map<User>(request);
            user.Id = Guid.NewGuid();
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _jwtService.GenerateToken(user);
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);

                var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
            {
                _logger.LogWarning("Login failed for {Email}", request.Email);
                throw new UnauthorizedException("Invalid email or password.");
            }
            return _jwtService.GenerateToken(user);
        }

        public async Task<string> GenerateResetPasswordTokenAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
                throw new BadRequestException("User not found");

            var restToken = _jwtService.GeneratePasswordResetToken(email);

            return restToken;
        }

        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);
            try
            {
                var claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = _config["JwtSettings:Issuer"],
                    ValidAudience = _config["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out _);

                var emailFromToken = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

                if(emailFromToken != email || emailFromToken == null)
                    throw new BadRequestException("Invalid token or email mismatch.");

                var user = await _unitOfWork.Users.GetByEmailAsync(email);

                if(user == null)
                    throw new BadRequestException("User not found");

                user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (SecurityTokenException)
            {
                throw new BadRequestException("Invalid or expired reset token.");
            }
        }
    }
}
