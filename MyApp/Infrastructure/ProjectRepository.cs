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

    public async Task<IReadOnlyCollection<ProjectDTO>> ReadAsync()
    {
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

        
        /*_context.Projects in _context.Characters
                         where c.Id == characterId
                         select new CharacterDetailsDto(
                             c.Id,
                             c.GivenName,
                             c.Surname,
                             c.AlterEgo,
                             c.City == null ? null : c.City.Name,
                             c.Gender,
                             c.FirstAppearance,
                             c.Occupation,
                             c.ImageUrl,
                             c.Powers.Select(c => c.Name).ToHashSet()
                         );

        return await characters.FirstOrDefaultAsync();


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
        ).AsReadOnly();*/
    }

    public async Task<StatusIndicator<ProjectDTO>> ReadAsync(int projectId)
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

    public Task<StatusIndicator<IReadOnlyCollection<ProjectDTO>>> ReadAsync(List<string> tags)
    {
        throw new NotImplementedException();
    }

    public async Task<StatusIndicator<IReadOnlyCollection<ProjectDTO>>> ReadAsync(string title)
    {
        
        var projects = (await _context.Projects.Where(p => p.Name == title).Select(p => new ProjectDTO(
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

        return (State.Found, projects);
    }
}