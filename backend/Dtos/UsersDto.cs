namespace Dto;

public record UserCreateRequest(string email, string username, string password);

public record UserData(int id, string email, string username, DateTime createdAt);
public record UserResponse(UserData data);

public record UserPublicData(string username, DateTime createdAt);
public record UserPublicResponse(UserPublicData data);

public record UserPublicList(List<UserPublicResponse> data);
