using Microsoft.AspNetCore.Mvc;

using Model;
using Service;

namespace Controller;

public record UserCreate(string email, string username, string password);

public record UserData(int id, string username, string email, DateTime createdAt);
public record UserCreated(UserData data);

public record ErrorDetail(string code, string message);
public record UserError(ErrorDetail error);

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType<UserCreated>(StatusCodes.Status201Created)]
	[ProducesResponseType<UserError>(StatusCodes.Status409Conflict)]
	public ActionResult<UserCreated> Post(UsersService service, [FromBody]UserCreate user)
	{
		User newUser;
		try {
			newUser = service.CreateUser(user.email, user.username, user.password);
		} catch(UsernameConflictException u) {
			return StatusCode(
					StatusCodes.Status409Conflict, new UserError(
						new ErrorDetail(
							"USERNAME_EXISTS",
							"username already in use by another account"
							)
						)
					);	
		} catch(EmailConflictException u) {
			return StatusCode(
					StatusCodes.Status409Conflict, new UserError(
						new ErrorDetail(
							"EMAIL_EXISTS",
							"email already in use by another account"
							)
						)
					);	
		}
		return StatusCode(
				StatusCodes.Status201Created, new UserCreated(
					new UserData(
						newUser.Id,
						newUser.Email,
						newUser.Username,
						newUser.CreatedAt
						)
					)
				);	
	}
}
