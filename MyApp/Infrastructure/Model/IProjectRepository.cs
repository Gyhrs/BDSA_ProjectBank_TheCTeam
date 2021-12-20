using MyApp.Shared;

namespace MyApp.Infrastructure;
public interface IProjectRepository
{
    Task<ProjectDTO> GetProjectFromIDAsync(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTagsAsync(List<string> tags);
    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromName(string title);
    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTagsAndNameAsync(List<string> tags, string title);
    Task<IReadOnlyCollection<ProjectDTO>> GetAllProjectsAsync();
    Task<(Status, ProjectDTO)> CreateProjectAsync(ProjectCreateDTO create);
    Task<Status> UpdateProjectAsync(int id, ProjectUpdateDTO project);
    Task<Status> DeleteProjectAsync(int projectId);
}