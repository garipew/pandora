using Microsoft.EntityFrameworkCore;

using Data;

namespace pandora.tests;

public class TestDbFactory
{
	private const string ConnectionString = "Host=localhost;Port=7357;Database=pandora;Username=pandora;Password=pandora";

	public static PandoraContext Create()
	{
		var con_str = Environment.GetEnvironmentVariable("ConnectionStrings__PandoraContext")
			?? ConnectionString;
		var options = new DbContextOptionsBuilder<PandoraContext>()
			.UseNpgsql(con_str)
			.Options;
		return new PandoraContext(options);
	}
}
