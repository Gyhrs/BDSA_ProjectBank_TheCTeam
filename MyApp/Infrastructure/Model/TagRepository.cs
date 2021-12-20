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
    public async Task<IReadOnlyCollection<TagDTO>> GetAllTagsAsync()
    {
        var tags = await (
            from p in _context.Tags
            select new TagDTO(
                p.Name,
                p.Projects != null ? p.Projects.Select(p => p.Id).ToList() : null
            )).ToListAsync();

        return tags.AsReadOnly();
    }
}