using Service;
using Model;

using pandora.tests.Infrastructure;

namespace pandora.tests;

public class UsersServiceTests : IClassFixture<DbFixture>
{
	public UsersServiceTests(DbFixture fix) { }

	[Fact]
	public void CreateUser_ShouldPersist()
	{
		var ctx = TestDbFactory.Create();
		var service = new UsersService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		service.CreateUser("test@mail.com", "test", "test");

		Assert.Equal(1, ctx.Users.Count());
		tx.Rollback();
	}

	[Fact]
	public void CreateUser_ShouldThrowOnEmailConflict()
	{
		var ctx = TestDbFactory.Create();
		var service = new UsersService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		service.CreateUser("test@mail.com", "test1", "test");

		Assert.Throws<EmailConflictException>(() => service.CreateUser("test@mail.com", "test", "test"));
		tx.Rollback();
	}

	[Fact]
	public void CreateUser_ShouldThrowOnUsernameConflict()
	{
		var ctx = TestDbFactory.Create();
		var service = new UsersService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		service.CreateUser("test@mail.com", "test", "test");

		Assert.Throws<UsernameConflictException>(() => service.CreateUser("test1@mail.com", "test", "test"));
		tx.Rollback();
	}

	[Fact]
	public void Login_ShouldThrowOnWrongCredentials()
	{
		var ctx = TestDbFactory.Create();
		var service = new UsersService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		service.CreateUser("test@mail.com", "test", "test");

		Assert.Throws<UserWrongLoginException>(() => service.Login("wrongLogin", "test"));
		Assert.Throws<UserWrongLoginException>(() => service.Login("test", "wrongPassword"));
		Assert.Throws<UserWrongLoginException>(() => service.Login("test@mail.com", "wrongPassword"));
		tx.Rollback();
	}

	[Fact]
	public void UpdateUser_ShouldThrowOnConflict()
	{
		var ctx = TestDbFactory.Create();
		var service = new UsersService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		service.CreateUser("test@mail.com", "test", "testpassword");
		User toUpdate = service.CreateUser("test2@mail.com", "test2", "testpassword");

		Assert.Throws<EmailConflictException>(() => service.UpdateUser(toUpdate.Id, "test@mail.com", "test2", "testpassword"));
		Assert.Throws<UsernameConflictException>(() => service.UpdateUser(toUpdate.Id, "test2@mail.com", "test", "testpassword"));

		tx.Rollback();
	}
}
