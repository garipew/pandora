using Dto;
using Data;
using Model;

namespace Service;

public class BooksService
{
	private PandoraContext _context;

	public BooksService(PandoraContext ctx)
	{
		_context = ctx;
	}

	public Book CreateBook(string title, string? description, string isbn, int pages, List<int> authorsId)
	{
		if(_context.Books.Where(b => b.Isbn == isbn).Any()) {
			throw new IsbnConflictException();
		}

		Book newBook = new Book();
		newBook.Title       = title;
		newBook.Description = description;
		newBook.Isbn        = isbn;
		newBook.Pages       = pages;

		_context.Books.Add(newBook);
		_context.SaveChanges();

		foreach(var authorId in authorsId) {
			AuthorBook ab = new();
			ab.BookId = newBook.Id;
			ab.AuthorId = authorId;

			_context.AuthorBooks.Add(ab);
		}

		_context.SaveChanges();

		return newBook;
	}

	public List<Book> GetBooks()
	{
		return _context.Books.ToList();
	}

	public Book GetBookById(int bookId)
	{
		Book? book = _context.Books.FirstOrDefault(a => a.Id == bookId);
		if(book == null) {
			throw new BookNotFoundException();
		}
		return book;
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

	public Book UpdateBook(int bookId, string isbn, int pages, string title, string? description)
	{
		Book book = GetBookById(bookId);
		if(_context.Books.Where(b => b.Id != book.Id)
				.Any(b => b.Isbn == isbn)) {
			throw new IsbnConflictException();
		}
		book.Isbn        = isbn;
		book.Pages       = pages;
		book.Title       = title;
		book.Description = description;
		_context.SaveChanges();

		return book;
	}

	public Book DeleteBook(int bookId)
	{
		Book book = GetBookById(bookId);

		_context.Books.Remove(book);
		_context.SaveChanges();

		return book;
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
}

public class BookNotFoundException : Exception
{
	public BookNotFoundException() : base() { }
}

public class BookTitleConflictException : Exception
{
	public BookTitleConflictException() : base() { }
}

public class IsbnConflictException : Exception
{
	public IsbnConflictException() : base() { }
}

public class CollectionConflictException : Exception
{
	public CollectionConflictException() : base() { }
}
