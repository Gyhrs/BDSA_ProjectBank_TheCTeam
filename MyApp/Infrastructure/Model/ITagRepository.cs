namespace MyApp.Infrastructure.Model;

public interface ITagRepository
{
    Task<IReadOnlyCollection<TagDTO>> GetAllTagsAsync();
}