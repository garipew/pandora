using Microsoft.EntityFrameworkCore;

using Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<PandoraContext>(opt =>
		opt.UseNpgsql(builder.Configuration.GetConnectionString("PandoraContext")));

// Add services to the container.

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

app.UseAuthorization();

app.MapControllers();

app.Run();
