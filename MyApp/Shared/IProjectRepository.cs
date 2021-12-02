namespace MyApp.Shared;

public interface IProjectRepository
{
    (Status, Task<ProjectDTO>) ReadAsync(int projectId);

    (Status, Task<IReadOnlyCollection<ProjectDTO>>) ReadAsync(List<string> tags);

    (Status, Task<IReadOnlyCollection<ProjectDTO>>) ReadAsync(string title);

    Task<IReadOnlyCollection<ProjectDTO>> ReadAsync();
}