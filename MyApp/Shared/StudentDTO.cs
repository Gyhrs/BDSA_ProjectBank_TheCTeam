namespace MyApp.Shared;

public record StudentDTO
(
    string Email,
    string Name,
    string Program,
    int? Project
) : UserDTO(Email, Name);