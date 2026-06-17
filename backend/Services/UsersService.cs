using Microsoft.AspNetCore.Identity;

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

	public User GetUserById(int id)
	{
		User? user = _context.Users.FirstOrDefault(u => u.Id == id);
		if(user == null) {
			throw new UserNotFoundException();
		}
		return user;
	}

	public User GetUserByName(string emailOrUsername)
	{
		User? user = _context.Users.FirstOrDefault(u => emailOrUsername == u.Email || emailOrUsername == u.Username);
		if(user == null) {
			throw new UserNotFoundException();
		}
		return user;
	}

	public User Login(string emailOrUsername, string password)
	{
		User user;
		try {
			user = GetUserByName(emailOrUsername);
		} catch(UserNotFoundException) {
			throw new UserWrongLoginException();
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
		throw new UserWrongLoginException();
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
		if(_context.Users.Where(u => u.Id != userId).Any(u => u.Email == email)) {
			throw new EmailConflictException();
		}
		if(_context.Users.Where(u => u.Id != userId).Any(u => u.Username == username)) {
			throw new UsernameConflictException();
		}
		User user = GetUserById(userId);
		user.Email = email;
		user.Username = username;
		user.PasswordHash = Hash(user, password);

		_context.SaveChanges();

		return user;
	}

	public User DeleteUser(int userId)
	{
		User user = GetUserById(userId);

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
		User user = GetUserByName(emailOrUsername);
		user.Role = role;
		_context.SaveChanges();
		return user;
	}
}

public class UserWrongLoginException : Exception
{
	public UserWrongLoginException() : base() { }
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
