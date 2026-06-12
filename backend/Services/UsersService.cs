using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Data;
using Model;

namespace Service;

public class UsersService
{
	private PandoraContext _context;
	private readonly PasswordHasher<User> _hasher = new();

	public UsersService(PandoraContext ctx)
	{
		_context = ctx;
	}

	public User? TryGetUser(string emailOrUsername, string password)
	{
		var candidates = _context.Users
			.Where(u => emailOrUsername == u.Email || emailOrUsername == u.Username)
			.ToList();
		foreach(var c in candidates) {
			switch(_hasher.VerifyHashedPassword(c, c.PasswordHash, password)) {
				case PasswordVerificationResult.Success:
					return c;
				case PasswordVerificationResult.SuccessRehashNeeded:
					c.PasswordHash = Hash(c, password);
					_context.SaveChanges();
					return c;
				case PasswordVerificationResult.Failed:
				default:
					break;
			}
		}
		return null;
	}

	public User CreateUser(string email, string username, string password)
	{
		if(_context.Users.Where(u => u.Email == email).Any()) {
			throw new EmailConflictException();
		}
		if(_context.Users.Where(u => u.Username == username).Any()) {
			throw new UsernameConflictException();
		}

		User newUser = new User();
		newUser.Email = email;
		newUser.Username = username;
		newUser.CreatedAt = DateTime.UtcNow;
		newUser.PasswordHash = Hash(newUser, password);

		_context.Users.Add(newUser);
		_context.SaveChanges();

		return newUser;
	}

	private string Hash(User user, string password)
	{
		return _hasher.HashPassword(user, password);
	}
}

public class UsernameConflictException : Exception
{
	public UsernameConflictException() : base() { }
}

public class EmailConflictException : Exception
{
	public EmailConflictException() : base() { }
}
