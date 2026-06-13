using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Model;
using Service;
using Dto;

namespace Controller;

[ApiController]
public class BoxesController : ControllerBase
{

	[Authorize]
	[HttpPost("/users/{userId:int}/boxes")]
	[ProducesResponseType<BoxCreateResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<BoxCreateResponse> Post(UsersService usersService, BoxesService boxesService, int userId, [FromBody]BoxCreateRequest req)
	{
		var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
		if(currentUserId == null) {
			return StatusCode(
					StatusCodes.Status401Unauthorized, new PandoraError(
						new ErrorData(
							"UNAUTHORIZED",
							"missing or invalid token"
							)
						)
					);
		}
		if(!currentUserId.Equals(userId.ToString())) {
			return StatusCode(
					StatusCodes.Status403Forbidden, new PandoraError(
						new ErrorData(
							"FORBIDDEN",
							"you do not have permission to access this resource"
							)
						)
					);
		}
		Box newBox;
		try {
			newBox = boxesService.CreateBox(userId, req.title, req.description);
		} catch(BoxTitleConflictException) {
			return StatusCode(
					StatusCodes.Status409Conflict, new PandoraError(
						new ErrorData(
							"BOX_EXISTS",
							"box already exists"
							)
						)
					);	
		}
		return StatusCode(
				StatusCodes.Status201Created, new BoxCreateResponse(
					new BoxData(
						newBox.Id,
						newBox.Title,
						newBox.Description
						)
					)
				);	
	}
}
