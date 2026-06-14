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

	public User? TryGetUserById(int id)
	{
		return _context.Users.FirstOrDefault(u => u.Id == id);
	}

	public User? TryGetUserByName(string emailOrUsername)
	{
		return _context.Users
			.FirstOrDefault(u => emailOrUsername == u.Email || emailOrUsername == u.Username);
	}

	public User? TryLogin(string emailOrUsername, string password)
	{
		User? user = TryGetUserByName(emailOrUsername);
		if(user == null) {
			return null;
		}
		switch(_hasher.VerifyHashedPassword(user, user.PasswordHash, password)) {
			case PasswordVerificationResult.Success:
				return user;
			case PasswordVerificationResult.SuccessRehashNeeded:
				user.PasswordHash = Hash(user, password);
				_context.SaveChanges();
				return user;
			case PasswordVerificationResult.Failed:
			default:
				break;
		}
		return null;
	}

	public List<User> GetUsers()
	{
		return _context.Users.ToList();
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
		newUser.Role = "User";
		// First user is assigned with Admin role
		if(!_context.Users.Any()) {
			newUser.Role = "Admin";
		}

		_context.Users.Add(newUser);
		_context.SaveChanges();

		return newUser;
	}

	public User UpdateUser(int userId, string email, string username, string password)
	{
		User? user = _context.Users.FirstOrDefault(u => u.Id == userId);
		if(user == null) {
			throw new UserNotFoundException();
		}

		user.Email = email;
		user.Username = username;
		user.PasswordHash = Hash(user, password);

		_context.SaveChanges();

		return user;
	}

	public User DeleteUser(int userId)
	{
		User? user = _context.Users.FirstOrDefault(u => u.Id == userId);
		if(user == null) {
			throw new UserNotFoundException();
		}

		_context.Users.Remove(user);
		_context.SaveChanges();

		return user;
	}

	private string Hash(User user, string password)
	{
		return _hasher.HashPassword(user, password);
	}

	public User AssignRole(string emailOrUsername, string role)
	{
		User? user = TryGetUserByName(emailOrUsername);
		if(user == null) {
			throw new UserNotFoundException();
		}
		user.Role = role;
		_context.SaveChanges();
		return user;
	}
}

public class UserNotFoundException : Exception
{
	public UserNotFoundException() : base() { }
}

public class UsernameConflictException : Exception
{
	public UsernameConflictException() : base() { }
}

public class EmailConflictException : Exception
{
	public EmailConflictException() : base() { }
}
