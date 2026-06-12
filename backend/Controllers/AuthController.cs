using Microsoft.AspNetCore.Mvc;

using Model;
using Dto;
using Service;

namespace Controller;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
	[HttpPost("login")]
	[ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	public ActionResult<AuthResponse> Post(UsersService usersService, AuthService authService, [FromBody]AuthRequest req)
	{
		User? user = usersService.TryGetUser(req.emailOrUsername, req.password);
		if(user == null) {
			return StatusCode(
					StatusCodes.Status401Unauthorized, new PandoraError(
						new ErrorData(
							"AUTH_ERROR",
							"username or password incorrect"
							)
						)
					);
		}
		return StatusCode(
				StatusCodes.Status200OK, new AuthResponse(
					new AuthData(
						user.Id,
						user.Username,
						authService.Authenticate(user)
						)
					)
				);
	}
}
