namespace Dto;

public record AuthorCreateRequest(string name);

public record AuthorData(int id, string name);
public record AuthorResponse(AuthorData data);

public record AuthorPublicData(string name);
public record AuthorPublicResponse(AuthorPublicData data);

public record AuthorPublicList(List<AuthorPublicData> data);
