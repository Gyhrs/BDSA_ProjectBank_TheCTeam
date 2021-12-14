using Microsoft.EntityFrameworkCore;
using MyApp.Shared;
using System.Linq;

namespace MyApp.Infrastructure;

public class TagRepository : ITagRepository
{
    private readonly IStudyBankContext _context;

    public TagRepository(IStudyBankContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<TagDTO>> GetAllTags()
    {
        var tags = await (
            from p in _context.Tags
            select new TagDTO(
                p.Name,
                p.Projects != null ? p.Projects.Select(p => p.Id).ToList() : null
            )).ToListAsync();

        return tags.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ProjectDTO>> GetProjectsFromTags(List<string> searchTags)
    {
        var tags = await _context.Tags.Where(t => searchTags.Any(ttag => ttag == t.Name)).ToListAsync();

        IEnumerable<Project> list = new List<Project>(tags.First().Projects);

        for (int i = 1; i < tags.Count; i++)
        {
            list = list.Intersect(tags.ElementAt(i).Projects);
        }

        var projects = list.Select(p => new ProjectDTO(
                            p.Id,
                            p.Name,
                            p.StartDate,
                            p.EndDate,
                            p.Description,
                            p.Students != null ? p.Students.Select(s => s.Email).ToList() : null,
                            p.Supervisors != null ? p.Supervisors.Select(s => s.Email).ToList() : null,
                            p.CreatedBy != null ? p.CreatedBy.Email : null,
                            p.CreatedBy != null ? p.CreatedBy.Name : null,
                            p.Tags != null ? p.Tags.Select(t => t.Name).ToList() : null
                        )).ToList();

        return projects.AsReadOnly();
    }
}