using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Model;
using Service;
using Dto;

namespace Controller;

[ApiController]
[Route("/users/{userId:int}/boxes")]
public class BoxesController : ControllerBase
{

	[HttpGet]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<BoxPublicList>(StatusCodes.Status200OK)]
	public ActionResult<BoxPublicList> Get(UsersService usersService, BoxesService boxesService, int userId)
	{
		User? user = usersService.TryGetUserById(userId);
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
		List<Box> boxes = boxesService.GetBoxes(userId);
		List<BoxPublicResponse> response = new();
		foreach(var box in boxes) {
			response.Add(new BoxPublicResponse(
						new BoxPublicData(
							box.Title
							)
						)
				    );
		}
		return StatusCode(StatusCodes.Status200OK, new BoxPublicList(
					response
					)
				);
	}

	[Authorize]
	[HttpPost]
	[ProducesResponseType<BoxCreateResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
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
		User? user = usersService.TryGetUserById(userId);
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
