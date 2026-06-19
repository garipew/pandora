using Data;
using Model;

namespace Service;

public class AuthorsService
{
	private PandoraContext _context;

	public AuthorsService(PandoraContext ctx)
	{
		_context = ctx;
	}

	public Author CreateAuthor(string name)
	{
		if(_context.Authors.Where(u => u.Name == name).Any()) {
			throw new AuthorNameConflictException();
		}

		Author newAuthor = new Author();
		newAuthor.Name = name;

		_context.Authors.Add(newAuthor);
		_context.SaveChanges();

		return newAuthor;
	}

	public List<Author> GetAuthors()
	{
		return _context.Authors.ToList();
	}

	public Author GetAuthorById(int authorId)
	{
		Author? author = _context.Authors.FirstOrDefault(a => a.Id == authorId);
		if(author == null) {
			throw new AuthorNotFoundException();
		}
		return author;
	}

	public Author GetOrCreate(string name)
	{
		Author? author = _context.Authors.FirstOrDefault(a => a.Name == name);
		if(author == null) {
			author = CreateAuthor(name);
		}
		return author;
	}

	public Author UpdateAuthor(int authorId, string name)
	{
		if(_context.Authors.Where(a => a.Id != authorId).Any(a => a.Name == name)) {
			throw new AuthorNameConflictException();
		}
		Author author = GetAuthorById(authorId);
		author.Name = name;
		_context.SaveChanges();

		return author;
	}

	public Author DeleteAuthor(int authorId)
	{
		Author author = GetAuthorById(authorId);

		List<Book> authorBooks = _context.AuthorBooks
			.Where(ab => ab.AuthorId == author.Id)
			.Join(_context.Books,
			ab => ab.BookId, book => book.Id,
			(ab, book) => book).ToList();

		foreach(var authorBook in authorBooks) {
			if(_context.AuthorBooks.Count(ab => ab.BookId == authorBook.Id) > 1) {
				continue;
			}
			_context.Books.Remove(authorBook);
		}

		_context.Authors.Remove(author);
		_context.SaveChanges();

		return author;
	}
}

public class AuthorNotFoundException : Exception
{
	public AuthorNotFoundException() : base() { }
}

public class AuthorNameConflictException : Exception
{
	public AuthorNameConflictException() : base() { }
}
