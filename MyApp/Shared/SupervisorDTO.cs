namespace MyApp.Shared;

public record SupervisorDTO
(
    string Email,
    string Name,
    List<int>? Projects
) : UserDTO(Email, Name);