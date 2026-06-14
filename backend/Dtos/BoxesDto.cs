namespace Dto;

public record BoxCreateRequest(string title, string? description);

public record BoxData(int id, string title, string? description);
public record BoxCreateResponse(BoxData data);

public record BoxPublicData(string title, string? description);
public record BoxPublicResponse(BoxPublicData data);

public record BoxPublicList(List<BoxPublicResponse> data);
