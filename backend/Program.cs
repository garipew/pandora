using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Service;
using Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(
		builder.Configuration.GetSection("JwtSettings")
		);
// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
			opt => builder.Configuration.Bind("JwtSettings", opt));

builder.Services.AddDbContextPool<PandoraContext>(opt =>
		opt.UseNpgsql(builder.Configuration.GetConnectionString("PandoraContext")));

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
