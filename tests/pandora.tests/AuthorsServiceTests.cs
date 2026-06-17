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
}
