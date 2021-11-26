namespace MyApp.Shared;

public record ProjectDTO
(
    int Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    string Description,
    List<string>? StudentEmails,
    List<string>? SupervisorsEmails,
    string? CreatedByEmail,
    List<string>? Tags
);