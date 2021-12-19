using MyApp.Shared;

namespace MyApp.Infrastructure;
public interface IProjectRepository
{
    Task<ProjectDTO> GetProjectFromID(int projectId);
    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTags(List<string> tags);
    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromName(string title);
    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTagsAndName(List<string> tags, string title);
    Task<IReadOnlyCollection<ProjectDTO>> GetAllProjects();
    Task<(Status, ProjectDTO)> CreateProject(ProjectCreateDTO create);
    Task<Status> UpdateProject(int id, ProjectUpdateDTO project);
    Task<Status> DeleteProject(int projectId);
}