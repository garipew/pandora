using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Data;
using Model;

namespace Service;

public class AuthService
{
	private readonly JwtSettings _settings;
	private PandoraContext _context;
	
	public AuthService(PandoraContext ctx, IOptions<JwtSettings> settings)
	{
		_context = ctx;
		_settings = settings.Value;
	}

	public string Authenticate(User user)
	{
		var claims = new[] {
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Name, user.Username),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.Role, user.Role),
		};

		var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(_settings.SecretKey)
				);
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
				issuer: _settings.Issuer,
				audience: _settings.Audience,
				claims: claims,
				signingCredentials: credentials
				);
		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}

public class JwtSettings
{
	public string SecretKey { get; set; }
	public string Issuer    { get; set; }
	public string Audience  { get; set; }
}
