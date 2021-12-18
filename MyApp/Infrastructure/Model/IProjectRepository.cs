namespace MyApp.Shared;

public interface IProjectRepository
{
    Task<ProjectDTO> GetProjectFromID(int projectId);

    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTags(List<string> tags);

    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromName(string title);

    Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTagsAndName(List<string> tags, string title);

    Task<IReadOnlyCollection<ProjectDTO>> GetAllProjects();

    Task<ProjectDTO> CreateProject(ProjectCreateDTO create);

}