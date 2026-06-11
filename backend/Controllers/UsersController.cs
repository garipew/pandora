using Microsoft.AspNetCore.Mvc;

using Model;
using Service;
using Dto;

namespace Controller;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType<UserCreateResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<UserCreateResponse> Post(UsersService service, [FromBody]UserCreateRequest user)
	{
		User newUser;
		try {
			newUser = service.CreateUser(user.email, user.username, user.password);
		} catch(UsernameConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"USERNAME_EXISTS",
							"username already in use by another account"
							)
						)
					);	
		} catch(EmailConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"EMAIL_EXISTS",
							"email already in use by another account"
							)
						)
					);	
		}
		return StatusCode(
				StatusCodes.Status201Created, new UserCreateResponse(
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
