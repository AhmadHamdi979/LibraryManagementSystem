using Library.Application.DTOs.Author;
using Library.Application.Features.Authors.Commands;
using Library.Application.Features.Authors.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var authors = await _mediator.Send(new GetAllAuthorsQuery(pageNumber, pageSize));

            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetById(Guid id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id));

            return Ok(author);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<AuthorDto>> CreateAuthor(CreateAuthorRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newAuthor = await _mediator.Send(new CreateAuthorCommand(request));

            return CreatedAtAction(nameof(GetById), new { id = newAuthor.Id }, newAuthor);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(UpdateAuthorRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateAuthorCommand(request));

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteAuthorCommand(id));

            return NoContent();
        }
    }
}
