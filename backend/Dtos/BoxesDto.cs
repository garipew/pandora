namespace Dto;

public record BoxCreateRequest(string title, string? description);

public record BoxData(int id, string title, string? description);
public record BoxCreateResponse(BoxData data);
