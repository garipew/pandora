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

	public Book CreateBook(string title, string? description, string isbn, int pages)
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
