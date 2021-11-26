namespace MyApp.Shared;

public interface IProjectRepository
{
    (State, Task<ProjectDTO>) ReadAsync(int projectId);

    (State, Task<IReadOnlyCollection<ProjectDTO>>) ReadAsync(List<string> tags);

    (State, Task<IReadOnlyCollection<ProjectDTO>>) ReadAsync(string title);

    Task<IReadOnlyCollection<ProjectDTO>> ReadAsync();
}