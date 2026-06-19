using Service;
using Model;

using pandora.tests.Infrastructure;

namespace pandora.tests;

public class BooksServiceTests : IClassFixture<DbFixture>
{
	public BooksServiceTests(DbFixture fix) { }

	[Fact]
	public void CreateBook_ShouldPersist()
	{
		var ctx = TestDbFactory.Create();
		var service = new BooksService(ctx);
		var authorsService = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author = authorsService.CreateAuthor("test author");
		List<int> authorsIds = new();

		authorsIds.Add(author.Id);

		service.CreateBook("test", "test", "test", 0, authorsIds);

		Assert.Equal(1, ctx.Books.Count());
		tx.Rollback();
	}

	[Fact]
	public void CreateBook_ShouldCreateRelationship()
	{
		var ctx = TestDbFactory.Create();
		var service = new BooksService(ctx);
		var authorsService = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author = authorsService.CreateAuthor("test author");
		List<int> authorsIds = new();

		authorsIds.Add(author.Id);

		service.CreateBook("test", "test", "test", 0, authorsIds);

		Assert.Equal(1, ctx.AuthorBooks.Count());
		tx.Rollback();
	}

	[Fact]
	public void CreateBook_ShouldThrowOnIsbnConflict()
	{
		var ctx = TestDbFactory.Create();
		var service = new BooksService(ctx);
		var authorsService = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author = authorsService.CreateAuthor("test author");
		List<int> authorsIds = new();

		authorsIds.Add(author.Id);

		service.CreateBook("test", "test", "test", 0, authorsIds);

		Assert.Throws<IsbnConflictException>(() => service.CreateBook("test2", "test2", "test", 0, authorsIds));
		tx.Rollback();
	}

	[Fact]
	public void UpdateBook_ShouldThrowOnIsbnConflict()
	{
		var ctx = TestDbFactory.Create();
		var service = new BooksService(ctx);
		var authorsService = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author = authorsService.CreateAuthor("test author");
		List<int> authorsIds = new();

		authorsIds.Add(author.Id);

		service.CreateBook("test", "test", "test", 0, authorsIds);
		Book toUpdate = service.CreateBook("test2", "test2", "test2", 0, authorsIds);

		Assert.Throws<IsbnConflictException>(() => service.UpdateBook(toUpdate.Id, "test", 0, "test2", "test2"));

		tx.Rollback();
	}

	[Fact]
	public void DeleteBook_ShouldRemove()
	{
		var ctx = TestDbFactory.Create();
		var service = new BooksService(ctx);
		var authorsService = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author = authorsService.CreateAuthor("test author");
		List<int> authorsIds = new();

		authorsIds.Add(author.Id);

		Book book = service.CreateBook("test", "test", "test", 0, authorsIds);

		service.DeleteBook(book.Id);

		Assert.Equal(0, ctx.Books.Count());

		tx.Rollback();
	}

	[Fact]
	public void DeleteBook_ShouldRemoveRelationships()
	{
		var ctx = TestDbFactory.Create();
		var service = new BooksService(ctx);
		var authorsService = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author = authorsService.CreateAuthor("test author");
		List<int> authorsIds = new();

		authorsIds.Add(author.Id);

		Book book = service.CreateBook("test", "test", "test", 0, authorsIds);

		service.DeleteBook(book.Id);

		Assert.Equal(0, ctx.AuthorBooks.Count());

		tx.Rollback();
	}
}
