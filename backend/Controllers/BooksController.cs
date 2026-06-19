using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Model;
using Service;
using Dto;

namespace Controller;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{

	[HttpGet]
	[ProducesResponseType<BookPublicList>(StatusCodes.Status200OK)]
	public ActionResult<BookPublicList> Get(BooksService booksService)
	{
		List<Book> books = booksService.GetBooks();
		List<BookPublicData> response = new();
		foreach(var book in books) {
			response.Add(new BookPublicData(
						book.Isbn,
						book.Pages,
						book.Title,
						book.Description
						)
				    );
		}
		return StatusCode(StatusCodes.Status200OK, new BookPublicList(
					response
					)
				);
	}

	[Authorize(Roles="Admin")]
	[HttpPost]
	[ProducesResponseType<BookResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<BookResponse> Post(BooksService booksService, AuthorsService authorsService, [FromBody]BookCreateRequest req)
	{
		Book newBook;
		List<int> authorsIds = new();
		foreach(var author in req.authors) {
			authorsIds.Add(authorsService.GetOrCreate(author).Id);
		}
		try {
			newBook = booksService.CreateBook(req.title, req.description, req.isbn, req.pages, authorsIds);
		} catch(Exception e) when (e is BookTitleConflictException || e is IsbnConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"BOOK_EXISTS",
							"book already exists"
							)
						)
					);	
		}
		return StatusCode(
				StatusCodes.Status201Created, new BookResponse(
					new BookData(
						newBook.Id,
						newBook.Title,
						newBook.Description,
						newBook.Isbn,
						newBook.Pages
						)
					)
				);
	}

	[HttpGet("{bookId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<BookPublicResponse>(StatusCodes.Status200OK)]
	public ActionResult<BookPublicResponse> Get(BooksService bookService, int bookId)
	{
		Book book;
		try {
			book = bookService.GetBookById(bookId);
		} catch(BookNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"BOOK_NOT_FOUND",
							$"book [{bookId}] do not exist"
							)
						)
					);
		}
		return StatusCode(
				StatusCodes.Status200OK, new BookPublicResponse(
					new BookPublicData(
						book.Isbn,
						book.Pages,
						book.Title,
						book.Description
						)
					)
				);
	}

	[Authorize(Roles="Admin")]
	[HttpPut("{bookId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	[ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
	public ActionResult<BookResponse> UpdateBook(BooksService bookService, int bookId, [FromBody]BookCreateRequest req)
	{
		Book book;
		try {
			book = bookService.UpdateBook(bookId, req.isbn, req.pages, req.title, req.description);
		} catch(BookNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"BOOK_NOT_FOUND",
							$"book [{bookId}] do not exist"
							)
						)
					);
		} catch(BookTitleConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"BOOK_EXISTS",
							"book title already in use"
							)
						)
					);	
		} catch(IsbnConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"BOOK_EXISTS",
							"book isbn already in use"
							)
						)
					);	
		}

		return StatusCode(
				StatusCodes.Status200OK, new BookPublicResponse(
					new BookPublicData(
						book.Isbn,
						book.Pages,
						book.Title,
						book.Description
						)
					)
				);
	}

	[Authorize(Roles="Admin")]
	[HttpDelete("{bookId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<BookResponse>(StatusCodes.Status200OK)]
	public ActionResult<BookResponse> DeleteBook(BooksService bookService, int bookId)
	{
		Book book;
		try {
			book = bookService.DeleteBook(bookId);
		} catch(BookNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"BOOK_NOT_FOUND",
							$"book [{bookId}] do not exist"
							)
						)
					);
		}
		return StatusCode(
				StatusCodes.Status200OK, new BookResponse(
					new BookData(
						book.Id,
						book.Title,
						book.Description,
						book.Isbn,
						book.Pages
						)
					)
				);
	}
}
