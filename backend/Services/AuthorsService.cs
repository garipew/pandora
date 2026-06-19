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

		_context.Authors.Remove(author);
		// TODO(garipew): Should delete books **exclusively** from this author,
		// also delete ALL relationships
		// AuthorBook.Where(ab => ab.AuthorId == authorId)
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
