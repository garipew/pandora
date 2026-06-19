using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Model;
using Service;
using Dto;

namespace Controller;

[ApiController]
[Route("[controller]")]
public class AuthorsController : ControllerBase
{
	[HttpGet]
	[ProducesResponseType<AuthorPublicList>(StatusCodes.Status200OK)]
	public ActionResult<AuthorPublicList> Get(AuthorsService authorsService)
	{
		List<Author> authors = authorsService.GetAuthors();
		List<AuthorPublicData> response = new();
		foreach(var author in authors) {
			response.Add(new AuthorPublicData(
						author.Name
						)
				    );
		}
		return StatusCode(StatusCodes.Status200OK, new AuthorPublicList(
					response
					)
				);
	}

	[HttpGet("{authorId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<AuthorPublicResponse>(StatusCodes.Status200OK)]
	public ActionResult<AuthorPublicResponse> Get(AuthorsService authorService, int authorId)
	{
		Author author;
		try {
			author = authorService.GetAuthorById(authorId);
		} catch(AuthorNotFoundException) {
			return StatusCode(
				StatusCodes.Status404NotFound, new PandoraError(
					new ErrorData(
						"AUTHOR_NOT_FOUND",
						$"author [{authorId}] do not exist"
						)
					)
				 );
		}
		return StatusCode(
				StatusCodes.Status200OK, new AuthorPublicResponse(
					new AuthorPublicData(
						author.Name
						)
					)
				);
	}

	[Authorize(Roles="Admin")]
	[HttpDelete("{authorId:int}")]
	[ProducesResponseType<PandoraError>(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status403Forbidden)]
	[ProducesResponseType<PandoraError>(StatusCodes.Status404NotFound)]
	[ProducesResponseType<AuthorResponse>(StatusCodes.Status200OK)]
	public ActionResult<AuthorResponse> DeleteAuthor(AuthorsService authorService, int authorId)
	{
		Author author;
		try {
			author = authorService.DeleteAuthor(authorId);
		} catch(AuthorNotFoundException) {
			return StatusCode(
					StatusCodes.Status404NotFound, new PandoraError(
						new ErrorData(
							"AUTHOR_NOT_FOUND",
							$"author [{authorId}] do not exist"
							)
						)
					);
		}
		return StatusCode(
				StatusCodes.Status200OK, new AuthorResponse(
					new AuthorData(
						author.Id,
						author.Name
						)
					)
				);
	}
}
