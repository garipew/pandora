namespace Dto;

using Model;

public record UserBookCreateRequest(
		string title, string author,
		int pagesRead,
		int rating, Status status, 
		DateTime? beginDate,
		DateTime? finishDate
		);

public record UserBookUpdateRequest(
		int pagesRead,
		int rating, Status status,
		DateTime? beginDate,
		DateTime? finishDate
		);


public record UserBookData(
		int UserId, int BookId,
		string Title,
		int PagesRead, int Pages,
		int Rating, Status Status, 
		DateTime? BeginDate,
		DateTime? FinishDate
		);


public record UserBookResponse(UserBookData data);
public record UserBookList(List<UserBookData> data);
