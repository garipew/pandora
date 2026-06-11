namespace Dto;

public record UserCreateRequest(string email, string username, string password);

public record UserData(int id, string username, string email, DateTime createdAt);
public record UserCreateResponse(UserData data);
