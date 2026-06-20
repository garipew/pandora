using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Dto;
using Model;
using Service;
using Data;

namespace Controller;

[ApiController]
[Route("/users/{userId:int}/books")]
public class CollectionsController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType<UserBookList>(StatusCodes.Status200OK)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	public ActionResult<UserBookList> GetUserBooks(int userId, CollectionsService collectionsService)
	{
		List<UserBookData> books;
		try {
			books = collectionsService.GetBooksFromUser(userId);
		} catch(UserNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"USER_NOT_FOUND",
							$"user [{userId}] do not exist"
							)
						)
					);
		}
		return StatusCode(StatusCodes.Status200OK, new UserBookList(
					books
					)
				);
	}

	[Authorize]
	[HttpPost]
	[ProducesResponseType<UserBookResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<UserBookResponse> AddBookToCollection(int userId, UsersService usersService, CollectionsService collectionsService, [FromBody]UserBookCreateRequest req)
	{
		var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if(currentUserId == null) {
			return StatusCode(
					StatusCodes.Status401Unauthorized, new PandoraError(
						new ErrorData(
							"UNAUTHORIZED",
							"missing or invalid token"
							)
						)
					);
		}
		if(!currentUserId.Equals(userId.ToString())) {
			return StatusCode(
					StatusCodes.Status403Forbidden, new PandoraError(
						new ErrorData(
							"FORBIDDEN",
							"you do not have permission to access this resource"
							)
						)
					);
		}
		UserBookData newBook;
		try {
			User user = usersService.GetUserById(userId);
			newBook = collectionsService.AddToCollection(
					user,
					req.title, req.author,
					req.pagesRead,
					req.rating, req.status,
					req.beginDate, req.finishDate
					);
		} catch(UserNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"USER_NOT_FOUND",
							$"user [{userId}] do not exist"
							)
						)
					);
		} catch(AuthorNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"AUTHOR_NOT_FOUND",
							$"author [{req.author}] do not exist"
							)
						)
					);
		} catch(BookNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"BOOK_NOT_FOUND",
							$"book [{req.title} - {req.author}] do not exist"
							)
						)
					);
		} catch(CollectionConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"BOOK_ALREADY_IN_COLLECTION",
							$"user [{userId}] already added book [{req.title} - {req.author}] to collection"
							)
						)
					);
		}

		return StatusCode(
				StatusCodes.Status201Created, new UserBookResponse(
					new UserBookData(
						newBook.UserId, newBook.BookId,
						newBook.Title,
						newBook.PagesRead, newBook.Pages,
						newBook.Rating, newBook.Status,
						newBook.BeginDate, newBook.FinishDate
						)
					)
				);
	}

	[HttpGet("{bookId:int}")]
	[ProducesResponseType<UserBookResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	public ActionResult<UserBookResponse> Get(int userId, int bookId, CollectionsService bookService)
	{
		UserBookData book;
		try {
			book = bookService.GetBookFromUser(userId, bookId);
		} catch(BookNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"BOOK_NOT_FOUND",
							$"book [{bookId}] do not exist"
							)
						)
					);
		} catch(UserNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"USER_NOT_FOUND",
							$"user [{userId}] do not exist"
							)
						)
					);
		}
		return StatusCode(
				StatusCodes.Status200OK, new UserBookResponse(
					new UserBookData(
						book.UserId, book.BookId,
						book.Title,
						book.PagesRead, book.Pages,
						book.Rating, book.Status,
						book.BeginDate, book.FinishDate
						)
					)
				);
	}

	[Authorize]
	[HttpPut("{bookId:int}")]
	[ProducesResponseType<UserBookResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<UserBookResponse> UpdateBook(int userId, int bookId, CollectionsService bookService, [FromBody]UserBookUpdateRequest req)
	{
		UserBookData book;
		try {
			book = bookService.UpdateUserBook(userId, bookId,
					req.pagesRead, req.rating,
					req.status,
					req.beginDate, req.finishDate);
		} catch(UserNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"USER_NOT_FOUND",
							$"user [{userId}] do not exist"
							)
						)
					);
		} catch(BookNotInCollectionException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"BOOK_NOT_IN_COLLECTION",
							$"book [{bookId}] is not on user [{userId}] collection"
							)
						)
					);
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

		return StatusCode(StatusCodes.Status200OK, new UserBookResponse(book));
	}

	[Authorize]
	[HttpDelete("{bookId:int}")]
	[ProducesResponseType<UserBookResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	public ActionResult<BookResponse> DeleteBook(int userId, int bookId, CollectionsService bookService)
	{
		UserBookData book;
		try {
			book = bookService.DeleteUserBook(userId, bookId);
		} catch(BookNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"BOOK_NOT_FOUND",
							$"book [{bookId}] do not exist"
							)
						)
					);
		} catch(UserNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"USER_NOT_FOUND",
							$"user [{userId}] do not exist"
							)
						)
					);
		} catch(BookNotInCollectionException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"BOOK_NOT_IN_COLLECTION",
							$"book [{bookId}] is not on user [{userId}] collection"
							)
						)
					);
		}
		return StatusCode(StatusCodes.Status200OK, new UserBookResponse(book));
	}
}
