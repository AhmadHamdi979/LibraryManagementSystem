using Library.API.Resources;
using Library.Application.DTOs.Book;
using Library.Application.Features.Books.Commands;
using Library.Application.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ServiceStack.Messaging;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public BooksController(IMediator mediator, IStringLocalizer<SharedResource> localizer)
        {
            _mediator = mediator;
            _localizer = localizer;
        }
        
        [Authorize]
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
            string? title = null, Guid? authorId = null)
        {
            var books = await _mediator.Send(new GetAllBooksQuery(pageNumber, pageSize, title, authorId));

            return Ok(books);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(Guid id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));


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
            

            return CreatedAtAction(nameof(GetById), new { Id = newBook.Id },new {Message= _localizer["BookCreated"] ,newBook });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new UpdateBookCommand(request));

            return Ok(new { Message = _localizer["BookUpdated"] });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id));

            return Ok(new { Message = _localizer["BookDeleted"] });
        }

    }
}
