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
	public ActionResult<BoxPublicList> Get(BoxesService boxesService, int userId)
	{
		List<Box> boxes;
		try {
			boxes = boxesService.GetBoxes(userId);
		} catch(UserNotFoundException) {
			return StatusCode(
				StatusCodes.Status404NotFound, new PandoraError(
					new ErrorData(
						"USER_NOT_FOUND",
						$"user [{userId}] do not exist"
						)
					)
				 );
		}
		List<BoxPublicResponse> response = new();
		foreach(var box in boxes) {
			response.Add(new BoxPublicResponse(
						new BoxPublicData(
							box.Title,
							box.Description
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
	[ProducesResponseType<BoxResponse>(StatusCodes.Status201Created)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status409Conflict)]
	public ActionResult<BoxResponse> Post(BoxesService boxesService, int userId, [FromBody]BoxCreateRequest req)
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
		} catch(UserNotFoundException) {
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
				StatusCodes.Status201Created, new BoxResponse(
					new BoxData(
						newBox.Id,
						newBox.Title,
						newBox.Description
						)
					)
				);
	}

	[HttpGet("{boxId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<BoxPublicResponse>(StatusCodes.Status200OK)]
	public ActionResult<BoxPublicResponse> Get(BoxesService boxService, int userId, int boxId)
	{
		Box box;
		try {
			box = boxService.GetBoxById(userId, boxId);
		} catch(UserNotFoundException) {
			return StatusCode(
				StatusCodes.Status404NotFound, new PandoraError(
					new ErrorData(
						"USER_NOT_FOUND",
						$"user [{userId}] do not exist"
						)
					)
				 );
		} catch(BoxNotFoundException) {
			return StatusCode(
				StatusCodes.Status404NotFound, new PandoraError(
					new ErrorData(
						"BOX_NOT_FOUND",
						$"box [{boxId}] from user [{userId}] do not exist"
						)
					)
				 );
		}
		return StatusCode(
				StatusCodes.Status200OK, new BoxPublicResponse(
					new BoxPublicData(
						box.Title,
						box.Description
						)
					)
				);
	}

	[Authorize]
	[HttpPut("{boxId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
	public ActionResult<BoxResponse> UpdateBox(BoxesService boxService, int userId, int boxId, [FromBody]BoxCreateRequest req)
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
		Box box;
		try {
			box = boxService.UpdateBox(userId, boxId, req.title, req.description);
		} catch(UserNotFoundException) {
			return StatusCode(
				StatusCodes.Status404NotFound, new PandoraError(
					new ErrorData(
						"USER_NOT_FOUND",
						$"user [{userId}] do not exist"
						)
					)
				 );
		} catch(BoxNotFoundException) {
			return StatusCode(
				StatusCodes.Status404NotFound, new PandoraError(
					new ErrorData(
						"BOX_NOT_FOUND",
						$"box [{boxId}] from user [{userId}] do not exist"
						)
					)
				 );
		}
		return StatusCode(
				StatusCodes.Status200OK, new BoxPublicResponse(
					new BoxPublicData(
						box.Title,
						box.Description
						)
					)
				);
	}
}
