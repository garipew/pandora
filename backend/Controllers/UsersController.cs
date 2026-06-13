using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Model;
using Service;
using Dto;

namespace Controller;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

	[Authorize(Roles = "Admin")]
	[HttpPost("assign")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
	public ActionResult<UserResponse> Assign(UsersService service, [FromBody]UserPromoteRequest req)
	{
		User? user = service.TryGetUserByName(req.emailOrUsername);
		if(user == null) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"USER_NOT_FOUND",
							$"user [{req.emailOrUsername}] do not exist"
							)
						)
					);
		}
		service.AssignRole(user, req.role);
		return StatusCode(
				StatusCodes.Status200OK, new UserResponse(
					new UserData(
						user.Id,
						user.Email,
						user.Username,
						user.CreatedAt
						)
					)
				);
	}

	[HttpGet]
	[ProducesResponseType<UserPublicList>(StatusCodes.Status200OK)]
	public ActionResult<UserPublicList> Get(UsersService service)
	{
		List<User> users = service.GetUsers();
		List<UserPublicResponse> response = new();
		foreach(var user in users) {
			response.Add(new UserPublicResponse(
						new UserPublicData(
							user.Username,
							user.CreatedAt
							)
						)
				    );
		}
		return StatusCode(StatusCodes.Status200OK, new UserPublicList(
					response
					)
				);
	}

	[HttpGet("{userId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<UserPublicResponse>(StatusCodes.Status200OK)]
	public ActionResult<UserPublicResponse> Get(UsersService service, int userId)
	{
		User? user = service.TryGetUserById(userId);
		if(user == null) {
			return StatusCode(
				StatusCodes.Status404NotFound, new PandoraError(
					new ErrorData(
						"USER_NOT_FOUND",
						$"user [{userId}] do not exist"
						)
					)
				 );
		}
		return StatusCode(
				StatusCodes.Status200OK, new UserPublicResponse(
					new UserPublicData(
						user.Username,
						user.CreatedAt
						)
					)
				);
	}

	[HttpPost]
	[ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<UserResponse> Post(UsersService service, [FromBody]UserCreateRequest user)
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
				StatusCodes.Status201Created, new UserResponse(
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
