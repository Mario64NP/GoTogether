namespace GoTogether.Application.DTOs.Files;

public record FileRequest
(
    string FileName,
    string ContentType,
    Stream Content
);
