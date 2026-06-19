namespace Dto;

public record BookCreateRequest(string title, string? description, string isbn, int pages, List<string> authors);

public record BookData(int id, string title, string? description, string isbn, int pages);
public record BookResponse(BookData data);

public record BookPublicData(string isbn, int pages, string title, string? description);
public record BookPublicResponse(BookPublicData data);

public record BookPublicList(List<BookPublicData> data);
