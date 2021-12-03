namespace MyApp.Shared;

public interface IUserRepository
{
    
    Task<UserDTO> GetUserFromEmailAsync(string userEmail);

    Task<IReadOnlyCollection<UserDTO>> GetAllUsersAsync();

    Task<IReadOnlyCollection<UserDTO>> GetUsersFromProjectIDAsync(int projectId);
}