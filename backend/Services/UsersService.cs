using Microsoft.AspNetCore.Mvc;

using Data;
using Model;

namespace Service;

public class UsersService
{
	private PandoraContext _context;

	public UsersService(PandoraContext ctx)
	{
		_context = ctx;
	}

	public User? TryGetUser(string emailOrUsername, string password)
	{
		return _context.Users
			.Where(u => emailOrUsername == u.Email || emailOrUsername == u.Username)
			.Where(u => u.PasswordHash == Hash(password))
			.FirstOrDefault();
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
		newUser.PasswordHash = Hash(password);
		newUser.CreatedAt = DateTime.UtcNow;

		_context.Users.Add(newUser);
		_context.SaveChanges();

		return newUser;
	}

	private string Hash(string input)
	{
		return input;
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
