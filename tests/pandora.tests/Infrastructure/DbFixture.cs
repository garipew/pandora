using Microsoft.EntityFrameworkCore;

using pandora.tests;

namespace pandora.tests.Infrastructure;

public class DbFixture : IDisposable
{
	public DbFixture()
	{
		using var ctx = TestDbFactory.Create();
		ctx.Database.Migrate();
	}

	public void Dispose() { }
}
