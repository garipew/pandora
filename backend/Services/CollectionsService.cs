using Dto;
using Data;
using Model;

namespace Service;

public class CollectionsService
{
	private PandoraContext _context;

	public CollectionsService(PandoraContext ctx)
	{
		_context = ctx;
	}

	public List<UserBookData> GetBooksFromUser(int userId)
	{
		if(!_context.Users.Any(u => u.Id == userId)) {
			throw new UserNotFoundException();
		}
		return _context.UserBooks
			.Where(u => u.UserId == userId)
			.Join(_context.Books,
				ub => ub.BookId, book => book.Id,
				(ub, book) => new UserBookData(
					ub.UserId,
					book.Id,
					book.Title,
					ub.PagesRead,
					book.Pages,
					ub.Rating,
					ub.Status,
					ub.BeginDate,
					ub.FinishDate
				)
			     )
			.ToList();
	}

	public Book GetBookByAuthor(string title, string author)
	{
		if(!_context.Books.Any(b => b.Title == title)) {
			throw new BookNotFoundException();
		}
		if(!_context.Authors.Any(a => a.Name == author)) {
			throw new AuthorNotFoundException();
		}

		Book? book = _context.Authors.Where(a => a.Name == author)
			.Join(_context.AuthorBooks,
				author => author.Id, ab => ab.AuthorId,
				(author, ab) => ab
			     )
			.Join(_context.Books,
				ab => ab.BookId, book => book.Id,
				(ab, book) => book
			     )
			.FirstOrDefault(b => b.Title == title);
		if(book == null) {
			throw new BookNotFoundException();
		}
		return book;
	}

	public UserBookData AddToCollection(User u, string title, string author, int pagesRead, int rating, Status status, DateTime? beginDate, DateTime? finishDate)
	{
		Book b = GetBookByAuthor(title, author);

		if(_context.UserBooks.Any(ub => ub.UserId == u.Id && ub.BookId == b.Id)) {
			throw new CollectionConflictException();
		}

		UserBook ub   = new();
		ub.UserId     = u.Id;
		ub.BookId     = b.Id;
		ub.PagesRead  = pagesRead;
		ub.Rating     = rating;
		ub.Status     = status;
		ub.BeginDate  = beginDate;
		ub.FinishDate = finishDate;

		_context.UserBooks.Add(ub);
		_context.SaveChanges();

		return new UserBookData(
				ub.UserId, ub.BookId,
				b.Title,
				ub.PagesRead, b.Pages,
				ub.Rating, ub.Status,
				ub.BeginDate,
				ub.FinishDate
				);
	}

	public UserBookData GetBookFromUser(int userId, int bookId)
	{
		if(!_context.Users.Any(u => u.Id == userId)) {
			throw new UserNotFoundException();
		}
		UserBookData? book = _context.UserBooks
			.Where(u => u.UserId == userId)
			.Join(_context.Books,
				ub => ub.BookId, book => book.Id,
				(ub, book) => new UserBookData(
					ub.UserId,
					book.Id,
					book.Title,
					ub.PagesRead,
					book.Pages,
					ub.Rating,
					ub.Status,
					ub.BeginDate,
					ub.FinishDate
				)
			     )
			.ToList()
			.FirstOrDefault(b => b.BookId == bookId);

		if(book == null) {
			throw new BookNotFoundException();
		}
		return book;
	}

	public UserBookData UpdateUserBook(int userId, int bookId, int pagesRead, int rating, Status status, DateTime? beginDate, DateTime? finishDate)
	{
		if(!_context.Users.Any(u => u.Id == userId)) {
			throw new UserNotFoundException();
		}
		if(!_context.Books.Any(b => b.Id == bookId)) {
			throw new BookNotFoundException();
		}

		UserBook? ub = _context.UserBooks.FirstOrDefault(ub => ub.UserId == userId && ub.BookId == bookId);
		if(ub == null) {
			throw new BookNotInCollectionException();
		}

		ub.PagesRead = pagesRead;
		ub.Rating = rating;
		ub.Status = status;
		ub.BeginDate = beginDate;
		ub.FinishDate = finishDate;

		_context.SaveChanges();

		return GetBookFromUser(userId, bookId);
	}

	public UserBookData DeleteUserBook(int userId, int bookId)
	{
		if(!_context.Users.Any(u => u.Id == userId)) {
			throw new UserNotFoundException();
		}
		if(!_context.Books.Any(b => b.Id == bookId)) {
			throw new BookNotFoundException();
		}

		UserBook? ub = _context.UserBooks.FirstOrDefault(ub => ub.UserId == userId && ub.BookId == bookId);
		if(ub == null) {
			throw new BookNotInCollectionException();
		}

		UserBookData data = GetBookFromUser(userId, bookId);

		_context.UserBooks.Remove(ub);
		_context.SaveChanges();

		return data;
	}
}

public class BookNotInCollectionException : Exception
{
	public BookNotInCollectionException() : base() { }
}

public class CollectionConflictException : Exception
{
	public CollectionConflictException() : base() { }
}
