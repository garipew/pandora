using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Text;

using Service;
using Data;
using Dto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(
		builder.Configuration.GetSection("JwtSettings")
		);
// Add services to the container.
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(opt =>
	{
		opt.Events = new JwtBearerEvents
		{
			OnChallenge = async context =>
			{
				context.HandleResponse();
				context.Response.StatusCode = 401;
				context.Response.ContentType = "application/json";
				await context.Response.WriteAsJsonAsync(
						new PandoraError(new ErrorData(
								"UNAUTHORIZED",
								"missing or invalid token"
								)
							)
						);
			},
			OnForbidden = async context =>
			{
				context.Response.StatusCode = 403;
				context.Response.ContentType = "application/json";
				await context.Response.WriteAsJsonAsync(
						new PandoraError(new ErrorData(
								"FORBIDDEN",
								"you do not have permission to access this resource"
								)
							)
						);
			}
		};
		opt.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

			ValidateIssuer = true,
			ValidIssuer = jwtSettings.Issuer,

			ValidateAudience = true,
			ValidAudience = jwtSettings.Audience,

			ValidateLifetime = false
		};
	});

builder.Services.AddDbContextPool<PandoraContext>(opt =>
		opt.UseNpgsql(builder.Configuration.GetConnectionString("PandoraContext")));

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<BoxesService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthorsService>();
builder.Services.AddScoped<BooksService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
		{
			opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
					{
						Name = "Authorization",
						Type = SecuritySchemeType.Http,
						Scheme = "bearer",
						BearerFormat = "JWT",
						In = ParameterLocation.Header,
						Description = "Enter your JWT token"
					});
			opt.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
	});

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
