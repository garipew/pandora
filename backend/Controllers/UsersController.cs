using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using Dto;
using Model;
using Service;
using Data;

namespace Controller;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	[Authorize(Roles = "Admin")]
	[HttpPost("assign")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
	public ActionResult<UserResponse> Assign(UsersService service, [FromBody]UserPromoteRequest req)
	{
		User user;
		try {
			user = service.AssignRole(req.emailOrUsername, req.role);
		} catch(UserNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"USER_NOT_FOUND",
							$"user [{req.emailOrUsername}] do not exist"
							)
						)
					);
		}
		return StatusCode(
				StatusCodes.Status200OK, new UserResponse(
					new UserData(
						user.Id,
						user.Email,
						user.Username,
						user.CreatedAt
						)
					)
				);
	}

	[HttpGet]
	[ProducesResponseType<UserPublicList>(StatusCodes.Status200OK)]
	public ActionResult<UserPublicList> Get(UsersService service)
	{
		List<User> users = service.GetUsers();
		List<UserPublicData> response = new();
		foreach(var user in users) {
			response.Add(new UserPublicData(
						user.Username,
						user.CreatedAt
						)
				    );
		}
		return StatusCode(StatusCodes.Status200OK, new UserPublicList(
					response
					)
				);
	}

	[HttpGet("{userId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<UserPublicResponse>(StatusCodes.Status200OK)]
	public ActionResult<UserPublicResponse> Get(UsersService service, int userId)
	{
		User user;
		try {
			user = service.GetUserById(userId);
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
				StatusCodes.Status200OK, new UserPublicResponse(
					new UserPublicData(
						user.Username,
						user.CreatedAt
						)
					)
				);
	}

	[Authorize]
	[HttpPut("{userId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
	public ActionResult<UserResponse> UpdateUser(UsersService service, int userId, [FromBody]UserCreateRequest req)
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
		User user;
		try {
			user = service.UpdateUser(userId, req.email, req.username, req.password);
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
				StatusCodes.Status200OK, new UserResponse(
					new UserData(
						user.Id,
						user.Email,
						user.Username,
						user.CreatedAt
						)
					)
				);
	}

	[Authorize]
	[HttpDelete("{userId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
	public ActionResult<UserResponse> DeleteUser(UsersService service, int userId)
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
		User user;
		try {
			user = service.DeleteUser(userId);
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
				StatusCodes.Status200OK, new UserResponse(
					new UserData(
						user.Id,
						user.Email,
						user.Username,
						user.CreatedAt
						)
					)
				);
	}

	[HttpPost]
	[ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<UserResponse> Post(UsersService service, [FromBody]UserCreateRequest user)
	{
		User newUser;
		try {
			newUser = service.CreateUser(user.email, user.username, user.password);
		} catch(UsernameConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"USERNAME_EXISTS",
							"username already in use by another account"
							)
						)
					);	
		} catch(EmailConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"EMAIL_EXISTS",
							"email already in use by another account"
							)
						)
					);	
		}
		return StatusCode(
				StatusCodes.Status201Created, new UserResponse(
					new UserData(
						newUser.Id,
						newUser.Email,
						newUser.Username,
						newUser.CreatedAt
						)
					)
				);	
	}

	[HttpGet("{userId:int}/books")]
	[ProducesResponseType<UserBookList>(StatusCodes.Status200OK)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	public ActionResult<UserBookList> GetUserBooks(int userId, BooksService booksService)
	{
		List<UserBookData> books;
		try {
			books = booksService.GetBooksFromUser(userId);
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
	[HttpPost("{userId:int}/books")]
	[ProducesResponseType<BookResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<BookResponse> AddBookToCollection(int userId, UsersService usersService, BooksService booksService, [FromBody]UserBookCreateRequest req)
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
			newBook = booksService.AddToCollection(
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

	[HttpGet("{userId:int}/books/{bookId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<UserBookResponse>(StatusCodes.Status200OK)]
	public ActionResult<UserBookResponse> Get(int userId, int bookId, BooksService bookService)
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
}
