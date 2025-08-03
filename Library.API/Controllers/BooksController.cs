using Library.Application.DTOs.Book;
using Library.Application.Features.Books.Commands;
using Library.Application.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
            string? title = null, Guid? authorId = null)
        {
            var books = await _mediator.Send(new GetAllBooksQuery(pageNumber, pageSize, title, authorId));

            if (!books.Any())
                return NotFound(new { message = "No authors found." });

            return Ok(books);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(Guid id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));

            if (book == null)
                return NotFound("No Book Found");

            return Ok(book);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<BookDto>> CreateBook(CreateBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newBook = await _mediator.Send(new CreateBookCommand(request));

            if (newBook == null)
                return NotFound("Author does not exist");

            return CreatedAtAction(nameof(GetById), new { Id = newBook.Id }, newBook);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateBookCommand(request));

            if (!result)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id));

            if (!result)
                return NotFound();

            return NoContent();
        }

    }
}
