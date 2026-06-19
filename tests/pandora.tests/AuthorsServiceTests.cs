using Service;
using Model;

using pandora.tests.Infrastructure;

namespace pandora.tests;

public class AuthorsServiceTests : IClassFixture<DbFixture>
{
	public AuthorsServiceTests(DbFixture fix) { }

	[Fact]
	public void CreateAuthor_ShouldPersist()
	{
		var ctx = TestDbFactory.Create();
		var service = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		service.CreateAuthor("test author");

		Assert.Equal(1, ctx.Authors.Count());
		tx.Rollback();
	}

	[Fact]
	public void CreateAuthor_ShouldThrowOnConflict()
	{
		var ctx = TestDbFactory.Create();
		var service = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		service.CreateAuthor("test author");

		Assert.Throws<AuthorNameConflictException>(() => service.CreateAuthor("test author"));
		tx.Rollback();
	}

	[Fact]
	public void UpdateAuthor_ShouldThrowOnConflict()
	{
		var ctx = TestDbFactory.Create();
		var service = new AuthorsService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		service.CreateAuthor("test author");
		Author toUpdate = service.CreateAuthor("test author 2");

		Assert.Throws<AuthorNameConflictException>(() => service.UpdateAuthor(toUpdate.Id, "test author"));

		tx.Rollback();
	}

	[Fact]
	public void DeleteAuthor_ShouldRemove()
	{
		var ctx = TestDbFactory.Create();
		var service = new AuthorsService(ctx);
		var bService = new BooksService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author = service.CreateAuthor("test author");

		service.DeleteAuthor(author.Id);
		Assert.Equal(0, ctx.Authors.Count());

		tx.Rollback();
	}

	[Fact]
	public void DeleteAuthor_ShouldDeleteAuthorBooks()
	{
		var ctx = TestDbFactory.Create();
		var service = new AuthorsService(ctx);
		var bService = new BooksService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author = service.CreateAuthor("test author");

		List<int> authorsIds = new();

		authorsIds.Add(author.Id);

		bService.CreateBook("test book 1", "test description 1", "test isbn 1", 0, authorsIds);
		bService.CreateBook("test book 2", "test description 2", "test isbn 2", 0, authorsIds);
		bService.CreateBook("test book 3", "test description 3", "test isbn 3", 0, authorsIds);
		bService.CreateBook("test book 4", "test description 4", "test isbn 4", 0, authorsIds);

		service.DeleteAuthor(author.Id);

		Assert.Equal(0, ctx.AuthorBooks.Count());
		Assert.Equal(0, ctx.Books.Count());
		tx.Rollback();
	}

	[Fact]
	public void DeleteAuthor_ShouldNotDeleteSharedBooks()
	{
		var ctx = TestDbFactory.Create();
		var service = new AuthorsService(ctx);
		var bService = new BooksService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		Author author1 = service.CreateAuthor("test author 1");
		Author author2 = service.CreateAuthor("test author 2");

		List<int> authorsIds = new();

		authorsIds.Add(author1.Id);
		authorsIds.Add(author2.Id);

		bService.CreateBook("test shared book 1", "test description 1", "test isbn 1", 0, authorsIds);

		service.DeleteAuthor(author1.Id);

		Assert.Equal(1, ctx.AuthorBooks.Count());
		Assert.Equal(1, ctx.Books.Count());
		tx.Rollback();
	}
}
