namespace MyApp.Shared;

public interface IProjectRepository
{
    Task<ProjectDTO> GetProjectFromIDAsync(int projectId);

    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTagsAsync(List<string> tags);

    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromNameAsync(string title);

    Task<IReadOnlyCollection<ProjectDTO>> GetAllProjectsAsync();
}