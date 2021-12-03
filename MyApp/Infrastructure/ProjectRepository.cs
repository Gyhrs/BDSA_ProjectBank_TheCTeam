using Microsoft.EntityFrameworkCore;
using MyApp.Shared;
using System.Linq;

namespace MyApp.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    private readonly IStudyBankContext _context;

    public ProjectRepository(IStudyBankContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetAllProjectsAsync()
    {
        // Await simply allows the application to perform other actions while it waits for a response from the database.
        var projects = await (
                        from p in _context.Projects
                        select new ProjectDTO(
                            p.Id,
                            p.Name,
                            p.StartDate,
                            p.EndDate,
                            p.Description,
                            p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                            p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                            p.CreatedBy != null ? p.CreatedBy.Email : null,
                            p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                        )).ToListAsync();        

        return projects.AsReadOnly();
    }

    public async Task<ProjectDTO> GetProjectFromIDAsync(int projectId)
    {
        var projects = from p in _context.Projects
                        where p.Id == projectId
                        select new ProjectDTO(
                            p.Id,
                            p.Name,
                            p.StartDate,
                            p.EndDate,
                            p.Description,
                            p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                            p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                            p.CreatedBy != null ? p.CreatedBy.Email : null,
                            p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                        );
        return await projects.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTagsAsync(List<string> searchTags)
    {
        var projects = await (
                        from p in _context.Projects
                        where p.Tags.Any(t => searchTags.Contains(t.Name)) // If any projecttag is contained in list of searchtags, return true
                        select new ProjectDTO(
                            p.Id,
                            p.Name,
                            p.StartDate,
                            p.EndDate,
                            p.Description,
                            p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                            p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                            p.CreatedBy != null ? p.CreatedBy.Email : null,
                            p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                        )).ToListAsync();        

        return projects.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromNameAsync(string name)
    { 
        var projects = await (
                        from p in _context.Projects
                        where p.Name == name
                        select new ProjectDTO(
                            p.Id,
                            p.Name,
                            p.StartDate,
                            p.EndDate,
                            p.Description,
                            p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                            p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                            p.CreatedBy != null ? p.CreatedBy.Email : null,
                            p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                        )).ToListAsync();        

        return projects.AsReadOnly();
    }
}