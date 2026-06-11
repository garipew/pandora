namespace Dto;

public record ErrorData(string code, string message);

public record PandoraError(ErrorData error);
