using Model;
using Service;

using pandora.tests.Infrastructure;

namespace pandora.tests;

public class BoxesServiceTests : IClassFixture<DbFixture>
{
	public BoxesServiceTests(DbFixture fix) { }

	[Fact]
	public void CreateBox_ShouldPersist()
	{
		var ctx = TestDbFactory.Create();
		var userService = new UsersService(ctx);
		var service = new BoxesService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		User u = userService.CreateUser("test@mail.com", "test", "test");
		service.CreateBox(u.Id, "test box", "test description");

		Assert.Equal(1, ctx.Boxes.Count());
		tx.Rollback();
	}

	[Fact]
	public void UpdateBox_ShouldPersist()
	{
		var ctx = TestDbFactory.Create();
		var userService = new UsersService(ctx);
		var service = new BoxesService(ctx);

		using var tx = ctx.Database.BeginTransaction();

		User u = userService.CreateUser("test@mail.com", "test", "test");

		const string originalTitle = "test box";
		const string originalDescription = "test description";
		Box box = service.CreateBox(u.Id, originalTitle, originalDescription);

		const string updatedTitle = "updated box";
		const string updatedDescription = "updated description";
		service.UpdateBox(u.Id, box.Id, updatedTitle, updatedDescription);

		var updatedBox = ctx.Boxes.First(b => b.Id == box.Id);
		Assert.Equal(updatedTitle, updatedBox.Title); 
		Assert.Equal(updatedDescription, updatedBox.Description);

		tx.Rollback();
	}
}
