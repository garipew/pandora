namespace Dto;

public record AuthRequest(string emailOrUsername, string password);

public record AuthData(int id, string username, string token);
public record AuthResponse(AuthData data);
