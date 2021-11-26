using Microsoft.EntityFrameworkCore;
using MyApp.Shared;

namespace MyApp.Infrastructure;

public class ProjectRepository : IProjectRepository
{
    private readonly IStudyBankContext _context;

    public ProjectRepository(IStudyBankContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAsync()
    {
        return (
            await _context.Projects.Select(p => new ProjectDTO(
                    p.Id,
                    p.Name,
                    p.StartDate,
                    p.EndDate,
                    p.Description,
                    p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                    p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                    p.CreatedBy != null ? p.CreatedBy.Email : null,
                    p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                )
            ).ToListAsync()
        ).AsReadOnly();
    }

    public (State, Task<ProjectDTO>) ReadAsync(int projectId)
    {
        throw new NotImplementedException();
    }

    public (State, Task<IReadOnlyCollection<ProjectDTO>>) ReadAsync(List<string> tags)
    {
        throw new NotImplementedException();
    }

    public (State, Task<IReadOnlyCollection<ProjectDTO>>) ReadAsync(string title)
    {
        throw new NotImplementedException();
    }

    
}