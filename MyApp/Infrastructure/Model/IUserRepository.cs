namespace MyApp.Infrastructure.Model;
public interface IUserRepository
{
    Task<UserDTO> GetUserFromEmailAsync(string userEmail);

    Task<IReadOnlyCollection<UserDTO>> GetAllUsersAsync();

    Task<IReadOnlyCollection<StudentDTO>> GetStudentsFromProjectIDAsync(int projectId);
    Task<IReadOnlyCollection<SupervisorDTO>> GetSupervisorsFromProjectIDAsync(int projectId);
}