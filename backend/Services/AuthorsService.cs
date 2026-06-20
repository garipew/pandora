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
		var capitalizedName = name.ToLower();
		if(_context.Authors.Where(u => u.Name == capitalizedName).Any()) {
			throw new AuthorNameConflictException();
		}

		Author newAuthor = new Author();
		newAuthor.Name = capitalizedName;

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
		var capitalizedName = name.ToLower();
		Author? author = _context.Authors.FirstOrDefault(a => a.Name == capitalizedName);
		if(author == null) {
			author = CreateAuthor(capitalizedName);
		}
		return author;
	}

	public Author UpdateAuthor(int authorId, string name)
	{
		var capitalizedName = name.ToLower();
		if(_context.Authors.Where(a => a.Id != authorId).Any(a => a.Name == capitalizedName)) {
			throw new AuthorNameConflictException();
		}
		Author author = GetAuthorById(authorId);
		author.Name = capitalizedName;
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
