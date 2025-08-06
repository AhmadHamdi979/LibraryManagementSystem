using Library.Application.DTOs.Auth;
using Library.Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator , ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            _logger.LogInformation("User registration attempt for Email: {Email}", request.Email);

            var token = await _mediator.Send(new RegisterUserCommand(request));

            _logger.LogInformation("User registered successfully: {Email}", request.Email);

            return Ok(new { Token = token });
        }

        [HttpPost("Login")] 
        public async Task<IActionResult> Login(LoginRequest request)
        {
            _logger.LogInformation("Login attempt for Email: {Email}", request.Email);

            var token = await _mediator.Send(new LoginUserCommand(request));

            _logger.LogInformation("Login successful for Email: {Email}", request.Email);

            return Ok(new { Token = token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var token = await _mediator.Send(new ForgotPasswordCommand(request));
       
            return Ok(new { ResetToken = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            await _mediator.Send(new ResetPasswordCommand(request));

            return Ok("Password reset successfully.");
        }
    }
}
